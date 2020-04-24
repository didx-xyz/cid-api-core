using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.Utilities;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Authentication;
using CoviIDApiCore.V1.DTOs.Clickatell;
using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Repositories;
using CoviIDApiCore.V1.Interfaces.Services;
using Hangfire;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Services
{
    public class OtpService : IOtpService
    {
        private readonly IConfiguration _configuration;
        private readonly IOtpTokenRepository _otpTokenRepository;
        private readonly IClickatellBroker _clickatellBroker;
        private readonly IConnectionService _connectionService;
        private readonly ICredentialService _credentialService;
        private readonly ICustodianBroker _custodianBroker;

        public OtpService(IOtpTokenRepository tokenRepository, IConfiguration configuration, IClickatellBroker clickatellBroker, ICustodianBroker custodianBroker, ICredentialService credentialService, IConnectionService connectionService)
        {
            _otpTokenRepository = tokenRepository;
            _configuration = configuration;
            _clickatellBroker = clickatellBroker;
            _custodianBroker = custodianBroker;
            _credentialService = credentialService;
            _connectionService = connectionService;
        }

        public async Task GenerateAndSendOtpAsync(string mobileNumber, Wallet wallet)
        {
            var expiryTime = _configuration.GetValue<int>("OTPSettings:ValidityPeriod");

            var code = Utilities.Helpers.GenerateRandom4DigitNumber();

            var message = ConstructMessage(mobileNumber, code, expiryTime, wallet);

            await _clickatellBroker.SendSms(message);

            await SaveOtpAsync(mobileNumber, code, expiryTime, wallet);
        }

        private ClickatellTemplate ConstructMessage(string mobileNumber, int code, int validityPeriod, Wallet wallet)
        {
            var recipient = new []
            {
                mobileNumber
            };

            return new ClickatellTemplate()
            {
                To = recipient,
                Content = string.Format(_configuration.GetValue<string>("OTPSettings:Message"), code.ToString(), validityPeriod.ToString())
            };
        }

        private async Task SaveOtpAsync(string mobileNumber, int code, int expiryTime, Wallet wallet)
        {
            var newToken = new OtpToken()
            {
                Wallet = wallet,
                Code = code,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMinutes(expiryTime),
                isUsed = false,
                MobileNumber = mobileNumber
            };

            await _otpTokenRepository.AddAsync(newToken);

            await _otpTokenRepository.SaveAsync();
        }

        public async Task ConfirmOtpAsync(RequestOtpConfirmation payload)
        {
            var token = await _otpTokenRepository.GetUnusedByWalletIdAndMobileNumber(payload.WalletId, payload.Person.MobileNumber.ToString());

            if(token == default || token.ExpireAt <= DateTime.UtcNow || token.Code != payload.Otp)
                throw new ValidationException(Messages.Token_OTPNotExist);

            token.isUsed = true;

            _otpTokenRepository.Update(token);

            await _otpTokenRepository.SaveAsync();

            await AddCredentialsToWallet(payload.CovidTest, payload.Person, payload.WalletId);
        }

        private async Task AddCredentialsToWallet(CovidTestCredentialParameters covidTest, PersonCredentialParameters person, string walletId)
        {
            var connectionParameters = new ConnectionParameters
            {
                ConnectionId = "", // Leave blank for auto generation
                Multiparty = false,
                Name = "CoviID", // This is the Agent name
            };

            var agentInvitation = await _connectionService.CreateInvitation(connectionParameters);

            var custodianConnection = await _connectionService.AcceptInvitation(agentInvitation.Invitation, walletId);

            var personalDetailsCredentials = await _credentialService.CreatePerson(agentInvitation.ConnectionId, person);

            if (covidTest != null)
            {
                var covidTestCredentials = await _credentialService.CreateCovidTest(agentInvitation.ConnectionId, covidTest);
            }

            var userCredentials = await _custodianBroker.GetCredentials(walletId);

            var offeredCredentials = userCredentials.Where(x => x.State == CredentialsState.Offered);

            foreach (var offer in offeredCredentials)
            {
                await _custodianBroker.AcceptCredential(walletId, offer.CredentialId);
            }
        }
    }
}
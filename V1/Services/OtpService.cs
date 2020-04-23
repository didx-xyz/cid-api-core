using System;
using System.Threading.Tasks;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.Utilities;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Authentication;
using CoviIDApiCore.V1.DTOs.Clickatell;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Repositories;
using CoviIDApiCore.V1.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Services
{
    public class OtpService : IOtpService
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;
        private readonly IClickatellBroker _clickatellBroker;

        public OtpService(ITokenRepository tokenRepository, IConfiguration configuration, IClickatellBroker clickatellBroker)
        {
            _tokenRepository = tokenRepository;
            _configuration = configuration;
            _clickatellBroker = clickatellBroker;
        }

        public async Task GenerateAndSendOtpAsync(string mobileNumber)
        {
            var expiryTime = _configuration.GetValue<int>("OTPSettings:ValidityPeriod");

            var code = Helpers.GenerateRandom4DigitNumber();

            var message = ConstructMessage(mobileNumber, code, expiryTime);

            await _clickatellBroker.SendSms(message);

            await SaveOtpAsync(mobileNumber, code, expiryTime);
        }

        private ClickatellTemplate ConstructMessage(string mobileNumber, int code, int validityPeriod)
        {
            var recipient = new []
            {
                mobileNumber
            };

            return new ClickatellTemplate()
            {
                To = recipient,
                Content = string.Format(_configuration.GetValue<string>("OTPSettings:Message"), code, validityPeriod),
                ValidityPeriod = validityPeriod,
                CharSet = "UTF-8"
            };
        }

        private async Task SaveOtpAsync(string mobileNumber, int code, int expiryTime)
        {
            var newToken = new Token()
            {
                Code = code,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMinutes(expiryTime),
                isUsed = false,
                MobileNumber = mobileNumber
            };

            await _tokenRepository.AddAsync(newToken);

            await _tokenRepository.SaveAsync();
        }

        public async Task ConfirmOtpAsync(RequestOtpConfirmation payload)
        {
            var token = await _tokenRepository.GetUnusedByMobileNumber(payload.MobileNumber);

            if(token == default || token.ExpireAt >= DateTime.UtcNow || token.Code != payload.Otp)
                throw new ValidationException(Messages.Token_OTPNotExist);

            token.isUsed = true;

            _tokenRepository.Update(token);
        }
    }
}
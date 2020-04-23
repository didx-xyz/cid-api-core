using System;
using System.Threading.Tasks;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.Utilities;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Authentication;
using CoviIDApiCore.V1.Interfaces.Repositories;
using CoviIDApiCore.V1.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Services
{
    public class OtpService : IOtpService
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;

        public OtpService(ITokenRepository tokenRepository, IConfiguration configuration)
        {
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        public async Task<int> GenerateOtpAsync(string mobileNumber)
        {
            var expiryTime = _configuration.GetValue<int>("OTPSettings:ValidityPeriod");

            var code = Helpers.GenerateRandom4DigitNumber();

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

            return code;
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
using System.Threading.Tasks;

using CoviIDApiCore.V1.DTOs.Authentication;
using CoviIDApiCore.V1.Interfaces.Repositories;
using CoviIDApiCore.V1.Interfaces.Services;

namespace CoviIDApiCore.V1.Services
{
    public class OtpService : IOtpService
    {
        private readonly ITokenRepository _tokenRepository;

        public OtpService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task ConfirmOtpAsync(RequestOtpConfirmation payload)
        {
        }
    }
}
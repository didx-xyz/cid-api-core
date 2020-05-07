using System.Threading.Tasks;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.DTOs.Authentication;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IOtpService
    {
        Task GenerateAndSendOtpAsync(string mobileNumber, Wallet wallet);
        Task ResendOtp(RequestResendOtp payload);
        Task ConfirmOtpAsync(RequestOtpConfirmation payload);
    }
}
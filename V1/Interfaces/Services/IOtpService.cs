using System.Threading.Tasks;
using CoviIDApiCore.V1.DTOs.Authentication;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IOtpService
    {
        Task GenerateAndSendOtpAsync(string mobileNumber);
        Task ConfirmOtpAsync(RequestOtpConfirmation payload);
    }
}
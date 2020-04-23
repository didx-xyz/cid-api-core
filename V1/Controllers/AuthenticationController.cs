using System.Threading.Tasks;
using CoviIDApiCore.V1.DTOs.Authentication;
using CoviIDApiCore.V1.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoviIDApiCore.V1.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IOtpService _otpService;

        public AuthenticationController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("otp")]
        public async Task<IActionResult> ConfirmOtp([FromBody] RequestOtpConfirmation payload)
        {
            await _otpService.ConfirmOtpAsync(payload);

            return Ok();
        }
    }
}
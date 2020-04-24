using System;
using System.Threading.Tasks;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoviIDApiCore.V1.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : Controller
    {
        private readonly IOtpService _otpService;

        public TestController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("sendsms")]
        public async Task<IActionResult> SendSMS()
        {
            await _otpService.GenerateAndSendOtpAsync("+27765408650", new Wallet()
            {
                Id = Guid.Parse("FAB107B6-0030-40EC-B324-08D7E7942BA0"),
                WalletIdentifier = "C8nTHJ2wwsff5WWRewGULLQ4pDmg63BMm"
            });

            return Ok();
        }
    }
}
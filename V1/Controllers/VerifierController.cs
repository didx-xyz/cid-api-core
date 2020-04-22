using System;
using System.Net;
using System.Threading.Tasks;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.V1.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Controllers
{
    [Route("api/verifier/")]
    [ApiController]
    public class VerifierController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVerifyService _verifyService;

        public VerifierController(IVerifyService verificationService, IConfiguration configuration)
        {
            _verifyService = verificationService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("{walletId}/covid-credentials")]
        public async Task<IActionResult> GetCovidStatus(string walletId)
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return Unauthorized(Messages.Misc_Unauthorized);

            var response = await _verifyService.GetCredentials(walletId);

            return Ok(new Response(response, HttpStatusCode.OK));
        }

        private bool IsAuthorized(string apiKey)
        {
            return string.Equals(apiKey, _configuration.GetValue<string>("Authorization"), StringComparison.Ordinal);
        }
    }
}
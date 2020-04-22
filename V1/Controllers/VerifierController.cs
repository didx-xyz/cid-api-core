using System.Net;
using System.Threading.Tasks;

using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.V1.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;

namespace CoviIDApiCore.V1.Controllers
{
    [Route("api/verifier/")]
    [ApiController]
    public class VerifierController : ControllerBase
    {
        private readonly IVerifyService _verifyService;

        public VerifierController(IVerifyService verificationService)
        {
            _verifyService = verificationService;
        }

        [HttpGet]
        [Route("{walletId}/covid-credentials")]
        public async Task<IActionResult> GetCovidStatus(string walletId)
        {
            var response = await _verifyService.GetCredentials(walletId);
            return Ok(new Response(response, HttpStatusCode.OK));
        }
    }
}
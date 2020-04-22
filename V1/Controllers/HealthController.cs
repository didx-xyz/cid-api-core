using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/health")]
    [ApiController]
    public class HealthController : Controller
    {
        private readonly IConfiguration _configuration;

        public HealthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Health()
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return new UnauthorizedResult();

            return new OkResult();
        }

        private bool IsAuthorized(string apiKey)
        {
            return string.Equals(apiKey, _configuration.GetValue<string>("Authorization"), StringComparison.Ordinal);
        }
    }
}

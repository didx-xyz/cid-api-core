using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CoviIDApiCore.V1.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("")]
    [ApiController]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Health()
        {
            return new OkResult();
        }

        [HttpGet("sentry")]
        public IActionResult SentryTest()
        {
            throw new Exception("Sentry Test Exception");
        }
    }
}

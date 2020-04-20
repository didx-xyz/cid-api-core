using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CoviIDApiCore.V1.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/health")]
    [ApiController]
    public class HealthController
    {
        [HttpGet]
        public IActionResult Health()
        {
            return new OkResult();
        }
    }
}

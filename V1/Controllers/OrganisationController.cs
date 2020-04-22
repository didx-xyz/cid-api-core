using System;
using System.Threading.Tasks;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Organisation;
using CoviIDApiCore.V1.Interfaces.Services;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/organisation")]
    [ApiController]
    public class OrganisationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOrganisationService _organisationService;

        public OrganisationController(IOrganisationService organisationService, IConfiguration configuration)
        {
            _organisationService = organisationService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganisation([FromBody] CreateOrganisationRequest payload)
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return Unauthorized(Messages.Misc_Unauthorized);

            await _organisationService.CreateAsync(payload);

            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganisation(string id)
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return Unauthorized(Messages.Misc_Unauthorized);

            var resp = await _organisationService.GetAsync(id);

            return StatusCode(resp.Meta.Code, resp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCount(string id, [FromBody] UpdateCountRequest payload)
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return Unauthorized(Messages.Misc_Unauthorized);

            var resp = await _organisationService.UpdateCountAsync(id, payload);

            return StatusCode(resp.Meta.Code, resp);
        }

        private bool IsAuthorized(string apiKey)
        {
            return string.Equals(apiKey, _configuration.GetValue<string>("Authorization"), StringComparison.Ordinal);
        }
    }
}
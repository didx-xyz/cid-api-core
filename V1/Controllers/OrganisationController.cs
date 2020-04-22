using System.Threading.Tasks;
using System.Net;
using CoviIDApiCore.V1.DTOs.Organisation;
using CoviIDApiCore.V1.Interfaces.Services;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CoviIDApiCore.V1.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/organisation")]
    [ApiController]
    public class OrganisationController : Controller
    {
        private readonly IOrganisationService _organisationService;

        public OrganisationController(IOrganisationService organisationService)
        {
            _organisationService = organisationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganisation([FromBody] CreateOrganisationRequest payload)
        {
            await _organisationService.CreateAsync(payload);

            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganisation(string id)
        {
            var resp = await _organisationService.GetAsync(id);

            return StatusCode(resp.Meta.Code, resp);
        }

        [HttpPut("{id}/{number}")]
        public async Task<IActionResult> UpdateCount(string id, [FromBody] UpdateCountRequest payload)
        {
            var resp = await _organisationService.UpdateCountAsync(id, payload);

            return StatusCode(resp.Meta.Code, resp);
        }
    }
}
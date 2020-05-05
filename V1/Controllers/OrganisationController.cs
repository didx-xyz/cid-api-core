using System.Net;
using System.Threading.Tasks;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Organisation;
using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.V1.Interfaces.Services;
using Hangfire;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            BackgroundJob.Enqueue(() => _organisationService.CreateAsync(payload));

            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganisation(string id)
        {
            var resp = await _organisationService.GetAsync(id);

            return StatusCode(resp.Meta.Code, resp);
        }

        [HttpPut("check_in")]
        public async Task<IActionResult> CheckIn([FromBody] UpdateCountRequest payload)
        {
            return StatusCode(StatusCodes.Status200OK,
                await _organisationService.UpdateCountAsync(payload, ScanType.CheckIn));
        }

        [HttpPut("check_out")]
        public async Task<IActionResult> CheckOut([FromBody] UpdateCountRequest payload)
        {
            return StatusCode(StatusCodes.Status200OK,
                await _organisationService.UpdateCountAsync(payload, ScanType.CheckOut));
        }

        [HttpPut("denied")]
        public async Task<IActionResult> AccessDenied([FromBody] UpdateCountRequest payload)
        {
            return StatusCode(StatusCodes.Status200OK,
                await _organisationService.UpdateCountAsync(payload, ScanType.Denied));
        }
    }
}
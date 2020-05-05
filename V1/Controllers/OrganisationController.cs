﻿using System.Net;
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
    [Route("api/organisations")]
    [ApiController]
    public class OrganisationController : Controller
    {
        private readonly IOrganisationService _organisationService;

        public OrganisationController(IOrganisationService organisationService)
        {
            _organisationService = organisationService;
        }

        [HttpPost]
        public IActionResult CreateOrganisation([FromBody] CreateOrganisationRequest payload)
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

        [HttpPost("{id}/check_in")]
        public async Task<IActionResult> CheckIn(string id, [FromBody] UpdateCountRequest payload)
        {
            return StatusCode(StatusCodes.Status200OK,
                await _organisationService.UpdateCountAsync(id, payload, ScanType.CheckIn));
        }

        [HttpPost("{id}/check_out")]
        public async Task<IActionResult> CheckOut(string id, [FromBody] UpdateCountRequest payload)
        {
            return StatusCode(StatusCodes.Status200OK,
                await _organisationService.UpdateCountAsync(id, payload, ScanType.CheckOut));
        }

        [HttpPost("{id}/denied")]
        public async Task<IActionResult> AccessDenied(string id, [FromBody] UpdateCountRequest payload)
        {
            return StatusCode(StatusCodes.Status200OK,
                await _organisationService.UpdateCountAsync(id, payload, ScanType.Denied));
        }
    }
}
﻿using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.V1.DTOs.Wallet;
using CoviIDApiCore.V1.Interfaces.Services;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService, IConfiguration configuration)
        {
            _walletService = walletService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return new UnauthorizedResult();

            var response = await _walletService.GetWallets();

            return Ok(new Response(response, HttpStatusCode.OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet(WalletParameters walletParameters)
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return new UnauthorizedResult();

            var response = await _walletService.CreateWallet(walletParameters);

            return Ok(new Response(response, HttpStatusCode.OK));
        }

        [HttpPost]
        [Route("coviid")]
        public async Task<IActionResult> CreateCoviIdWallet(CoviIdWalletParameters coviIdWalletParameters)
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return new UnauthorizedResult();

            var response = await _walletService.CreateCoviIdWallet(coviIdWalletParameters);

            return Ok(new Response(response, HttpStatusCode.OK));
        }

//        [HttpPut]
//        [Route("{walletId")]
//        public async Task<IActionResult> UpdateWallet(string walletId)
//        {
//             if(!IsAuthorized(Request.Headers["x-api-key"]))
//                return new UnauthorizedResult();
//            var response = await _walletService.CreateCoviIdWallet(walletId);
//            return Ok(new Response(response, HttpStatusCode.OK));
//        }

        [HttpDelete]
        [Route("{walletId}")]
        public async Task<IActionResult> DeleteWallet(string walletId)
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return new UnauthorizedResult();

            await _walletService.DeleteWallet(walletId);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteWallets(List<WalletParameters> wallets)
        {
            if (!IsAuthorized(Request.Headers["x-api-key"]))
                return new UnauthorizedResult();

            await _walletService.DeleteWallets(wallets);

            return Ok();
        }

        private bool IsAuthorized(string apiKey)
        {
            return string.Equals(apiKey, _configuration.GetValue<string>("Authorization"), StringComparison.Ordinal);
        }
    }
}
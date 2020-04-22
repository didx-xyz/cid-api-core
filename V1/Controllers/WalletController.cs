using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.V1.DTOs.Wallet;
using CoviIDApiCore.V1.Interfaces.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoviIDApiCore.V1.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var response = await _walletService.GetWallets();
            return Ok(new Response(response, HttpStatusCode.OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet(WalletParameters walletParameters)
        {
            var response = await _walletService.CreateWallet(walletParameters);
            return Ok(new Response(response, HttpStatusCode.OK));
        }

        [HttpPost]
        [Route("coviid")]
        public async Task<IActionResult> CreateCoviIdWallet(CoviIdWalletParameters coviIdWalletParameters)
        {
            var response = await _walletService.CreateCoviIdWallet(coviIdWalletParameters);
            return Ok(new Response(response, HttpStatusCode.OK));
        }

//        [HttpPut]
//        [Route("{walletId")]
//        public async Task<IActionResult> UpdateWallet(string walletId)
//        {
//            var response = await _walletService.CreateCoviIdWallet(walletId);
//            return Ok(new Response(response, HttpStatusCode.OK));
//        }

        [HttpDelete]
        [Route("{walletId}")]
        public async Task<IActionResult> DeleteWallet(string walletId)
        {
            await _walletService.DeleteWallet(walletId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteWallets(List<WalletParameters> wallets)
        {
            await _walletService.DeleteWallets(wallets);
            return Ok();
        }
    }
}
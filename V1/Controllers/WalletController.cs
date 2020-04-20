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
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        /// <summary>
        /// Get the wallets associated with the Agent/Tenant ID specified in the Header.
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>List of wallets</returns>
        [HttpGet]
        public async Task<IActionResult> GetWallet([FromHeader(Name = "X-AgentId")] string agentId)
        {
            var response = await _walletService.GetWallets(agentId);
            return Ok(new Response(response, HttpStatusCode.OK));
        }

        /// <summary>
        /// Creates a new wallet for a User associated to the specific Agent
        /// </summary>
        /// <param name="walletParameters"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateWallet(WalletParameters walletParameters, [FromHeader (Name = "X-AgentId")] string agentId)
        {
            var response = await _walletService.CreateWallet(walletParameters, agentId);
            return Ok(new Response(response, HttpStatusCode.OK));
        }

        /// <summary>
        /// Creates a new CoviID wallet and a users Covid Status(credentials) is set on this wallet and is hosted with the CoviID Agent
        /// </summary>
        /// <param name="walletParameters"></param>
        /// <param name="coviIDWalletParameters"></param>
        /// <param name="agentId"></param>
        /// <returns>The image uploaded and a link to the set of credentials</returns>
        [HttpPost]
        [Route("CoviID")]
        public async Task<IActionResult> CreateCoviIDWallet(CoviIDWalletParameters coviIDWalletParameters, [FromHeader(Name = "X-AgentId")] string agentId)
        {
            var response = await _walletService.CreateCoviIDWallet(coviIDWalletParameters, agentId);
            return Ok(new Response(response, HttpStatusCode.OK));
        }

        [HttpDelete]
        [Route("{walletId}")]
        public async Task<IActionResult> DeleteWallet(string walletId, [FromHeader(Name = "X-AgentId")] string agentId)
        {
            await _walletService.DeleteWallet(walletId, agentId);
            return Ok();
        }

        [HttpDelete]
        [Route("multiple")]
        public async Task<IActionResult> DeleteWallets(List<WalletParameters> wallets, [FromHeader(Name = "X-AgentId")] string agentId)
        {
            await _walletService.DeleteWallets(wallets, agentId);
            return Ok();
        }
    }
}
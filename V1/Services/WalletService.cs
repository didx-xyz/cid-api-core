using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Wallet;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Services
{
    public class WalletService : IWalletService
    {
        private readonly ICustodianBroker _custodianBroker;
        private readonly IAgencyBroker _agencyBroker;
        private readonly IConnectionService _connectionService;
        private readonly IConfiguration _configuration;
        public WalletService(ICustodianBroker custodianBroker, IConnectionService connectionService, IAgencyBroker agencyBroker, IConfiguration configuration)
        {
            _custodianBroker = custodianBroker;
            _connectionService = connectionService;
            _agencyBroker = agencyBroker;
            _configuration = configuration;
        }

        public async Task<List<WalletContract>> GetWallets(string agentId)
        {
            var response = await _custodianBroker.GetWallets(agentId);
            return response;
        }

        public async Task<WalletContract> CreateWallet(WalletParameters walletParameters, string agentId)
        {
            var respose = await _custodianBroker.CreateWallet(walletParameters, agentId);
            return respose;
        }

        public async Task<CoviIDWalletContract> CreateCoviIDWallet(CoviIDWalletParameters coviIDWalletParameters, string agentId)
        {
            // Create wallet
            var wallet = new WalletParameters
            {
                OwnerName = $"{coviIDWalletParameters.Name}-{coviIDWalletParameters.Surname}"
            };
            var response = await _custodianBroker.CreateWallet(wallet, agentId);

            //Upload image of the person
            var pictureUrl = await _agencyBroker.UploadFiles(coviIDWalletParameters.Picture, response.WalletId);

            // Not waiting for process to finish
            _ = ContinueProccess(coviIDWalletParameters, agentId, pictureUrl, response.WalletId);
            

            var contract = new CoviIDWalletContract
            {
                CovidStatusUrl = $"{_configuration.GetValue<string>("CoviIDBaseUrl")}/api/verifier/{response.WalletId}/covid-credentials",
                Picture = pictureUrl,
                WalletId = response.WalletId
            };

            return contract;

        }

        public async Task DeleteWallet(string walletId, string agentId)
        {
            await _custodianBroker.DeleteWallet(walletId, agentId);
            return;
        }

        public async Task DeleteWallets(List<WalletParameters> wallets, string agentId)
        {
            foreach (var wallet in wallets)
            {
                await _custodianBroker.DeleteWallet(wallet.WalletId, agentId);
            }
            return;
        }

        #region Private Methods
        /// <summary>
        /// Continues rest of process, of connecting to agent and creating the credentials
        /// </summary>
        /// <param name="coviIDWalletParameters"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        private async Task ContinueProccess(CoviIDWalletParameters coviIDWalletParameters, string agentId, string pictureUrl, string walletId)
        {
            var connectionParameters = new ConnectionParameters
            {
                // Leave blank for auto generation
                ConnectionId = "",
                Multiparty = false,
                // This is the Agent name
                Name = "CoviID",
            };

            // Creates an invitation to connect to agent and accepts the invitaion.
            var agentInvitation = await _connectionService.CreateInvitation(connectionParameters, agentId);
            var custodianConnection = await _connectionService.AcceptInvitation(agentInvitation.Invitation, walletId);

            //Create the set of credentials 
            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = agentInvitation.ConnectionId,
                DefinitionId = _configuration.GetValue<string>("DefinitionId"),
                AutomaticIssance = true,
                CredentialValues = new Dictionary<string, string>
                {
                    { "Name" , coviIDWalletParameters.Name },
                    { "Surname" , coviIDWalletParameters.Surname },
                    { "Picture" , pictureUrl},
                    { "TelNumber" , coviIDWalletParameters.TelNumber},
                    { "CovidStatus" , coviIDWalletParameters.CovidTest.CovidStatus.ToString()},
                    { "TestDate" , coviIDWalletParameters.CovidTest.TestDate.ToString()},
                    { "ExpiryDate" , coviIDWalletParameters.CovidTest.ExpiryDate.ToString()},
                }
            };
            // Create/send credentials
            var credentials = await _agencyBroker.SendCredentials(credentialOffer, agentId);

            //Get cred
            var userCredentials = await _custodianBroker.GetCredentials(walletId);

            var offeredCredentials = userCredentials.FirstOrDefault(x => x.State == CredentialsState.Offered);
            //Accept Credentials
            await _custodianBroker.AcceptCredential(walletId, offeredCredentials.CredentialId);
            return;
        }
        #endregion
    }

}


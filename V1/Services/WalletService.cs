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
        private readonly ICredentialService _credentialService;
        private readonly IConfiguration _configuration;
        public WalletService(ICustodianBroker custodianBroker, IConnectionService connectionService, IAgencyBroker agencyBroker,
            IConfiguration configuration,
            ICredentialService credentialService)
        {
            _custodianBroker = custodianBroker;
            _connectionService = connectionService;
            _agencyBroker = agencyBroker;
            _configuration = configuration;
            _credentialService = credentialService;
        }

        public async Task<List<WalletContract>> GetWallets()
        {
            return await _custodianBroker.GetWallets();
        }

        public async Task<WalletContract> CreateWallet(WalletParameters walletParameters)
        {
            return await _custodianBroker.CreateWallet(walletParameters);
        }

        public async Task<CoviIdWalletContract> CreateCoviIdWallet(CoviIdWalletParameters coviIdWalletParameters)
        {
            var wallet = new WalletParameters
            {
                OwnerName = $"{coviIdWalletParameters.PersonalDetials.Name}-{coviIdWalletParameters.PersonalDetials.Surname}"
            };

            var response = await _custodianBroker.CreateWallet(wallet);

            var pictureUrl = await _agencyBroker.UploadFiles(coviIdWalletParameters.PersonalDetials.Picture, response.WalletId);
            coviIdWalletParameters.PersonalDetials.Picture = pictureUrl;

            _ = ContinueProcess(coviIdWalletParameters, pictureUrl, response.WalletId);

            var contract = new CoviIdWalletContract
            {
                CovidStatusUrl = $"{_configuration.GetValue<string>("CoviIDBaseUrl")}/api/verifier/{response.WalletId}/covid-credentials",
                Picture = pictureUrl,
                WalletId = response.WalletId
            };

            return contract;
        }

        /// <summary>
        /// This will update the wallet with the newly added covid test results
        /// </summary>
        /// <returns></returns>
        public async Task UpdateWallet(CovidTestCredentialParameters covidTest, string walletId)
        {

            // 1. Get credentials
            // 2. Find existing covid credentials
            // 3. issue new credentials.
            // 4. revoke the previous ones

            //var credentials = await _custodianBroker.GetCredentials(walletId);
            return;
        }

        public async Task DeleteWallet(string walletId)
        {
            await _custodianBroker.DeleteWallet(walletId);
        }

        public async Task DeleteWallets(List<WalletParameters> wallets)
        {
            foreach (var wallet in wallets)
            {
                await _custodianBroker.DeleteWallet(wallet.WalletId);
            }
        }

        #region Private Methods
        public async Task ContinueProcess(CoviIdWalletParameters coviIdWalletParameters, string pictureUrl, string walletId)
        {
            var connectionParameters = new ConnectionParameters
            {
                ConnectionId = "", // Leave blank for auto generation
                Multiparty = false,
                Name = "CoviID", // This is the Agent name
            };

            var agentInvitation = await _connectionService.CreateInvitation(connectionParameters);
            var custodianConnection = await _connectionService.AcceptInvitation(agentInvitation.Invitation, walletId);

            // Create the set of credentials
            var personalDetialsCredentials = await _credentialService.CreatePersonalDetials(agentInvitation.ConnectionId, coviIdWalletParameters.PersonalDetials);
            var covidTestCredentials = await _credentialService.CreateCovidTest(agentInvitation.ConnectionId, coviIdWalletParameters.CovidTest);
            var identificationCredentials = await _credentialService.CreateIdentification(agentInvitation.ConnectionId, coviIdWalletParameters.Identification);

            var userCredentials = await _custodianBroker.GetCredentials(walletId);
            var offeredCredentials = userCredentials.Where(x => x.State == CredentialsState.Offered);

            if (offeredCredentials != null)
            {
                // Accept all the credentials
                foreach (var offer in offeredCredentials)
                {
                    await _custodianBroker.AcceptCredential(walletId, offer.CredentialId);
                }
            }
        }
        #endregion
    }
}
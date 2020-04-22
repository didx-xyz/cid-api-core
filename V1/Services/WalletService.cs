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
                OwnerName = $"{coviIdWalletParameters.Name}-{coviIdWalletParameters.Surname}"
            };

            var response = await _custodianBroker.CreateWallet(wallet);

            var pictureUrl = await _agencyBroker.UploadFiles(coviIdWalletParameters.Picture, response.WalletId);

            _ = ContinueProcess(coviIdWalletParameters, pictureUrl, response.WalletId);

            var contract = new CoviIdWalletContract
            {
                CovidStatusUrl = $"{_configuration.GetValue<string>("CoviIDBaseUrl")}/api/verifier/{response.WalletId}/covid-credentials",
                Picture = pictureUrl,
                WalletId = response.WalletId
            };

            return contract;
        }

        public async Task<WalletContract> UpdateWallet(string walletId)
        {
            //var credentials = await _custodianBroker.GetCredentials(walletId);
            return null;
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
        private async Task ContinueProcess(CoviIdWalletParameters coviIdWalletParameters, string pictureUrl, string walletId)
        {
            var connectionParameters = new ConnectionParameters
            {
                ConnectionId = "", // Leave blank for auto generation
                Multiparty = false,
                Name = "CoviID", // This is the Agent name
            };

            var agentInvitation = await _connectionService.CreateInvitation(connectionParameters);

            var custodianConnection = await _connectionService.AcceptInvitation(agentInvitation.Invitation, walletId);

            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = agentInvitation.ConnectionId,
                DefinitionId = _configuration.GetValue<string>("DefinitionId"),
                AutomaticIssuance = false,
                CredentialValues = new Dictionary<string, string>
                {
                    { "Name" , coviIdWalletParameters.Name },
                    { "Surname" , coviIdWalletParameters.Surname },
                    { "Picture" , pictureUrl},
                    { "TelNumber" , coviIdWalletParameters.TelNumber},
                    { "CovidStatus" , coviIdWalletParameters.CovidTest.CovidStatus.ToString()},
                    { "TestDate" , coviIdWalletParameters.CovidTest.TestDate.ToString()},
                    { "ExpiryDate" , coviIdWalletParameters.CovidTest.ExpiryDate.ToString()},
                }
            };

            var credentials = await _agencyBroker.SendCredentials(credentialOffer);

            var userCredentials = await _custodianBroker.GetCredentials(walletId);

            var offeredCredentials = userCredentials.FirstOrDefault(x => x.State == CredentialsState.Offered);

            if(offeredCredentials != null)
                await _custodianBroker.AcceptCredential(walletId, offeredCredentials.CredentialId);
        }
        #endregion
    }
}
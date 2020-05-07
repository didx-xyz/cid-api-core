using System;
using System.Threading.Tasks;
// TODO: All references to WalletContract below will change to reference the "new" version
using CoviIDApiCore.V1.DTOs.Wallet;
// TODO: All references to CovidTestCredentialParameters below will change to reference the "new" version
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string serverKey;

        public EncryptionService(IConfiguration configuration)
        {

            serverKey = configuration.GetValue<string>("ServerKey");
        }

        public Task<string> GenerateEncryptedSecretKey ()
        {
            // TODO
            return Task.FromResult<string>("totally_encrypted_secret_key");
        }

        public Task<WalletContract> EncryptWalletDetails(WalletContract plainTextWallet, string encryptedSecretKey)
        {
            // TODO
            var encryptedWallet = new WalletContract();
            return Task.FromResult<WalletContract>(encryptedWallet);
        }

        public Task<WalletContract> DecryptWalletDetails(WalletContract encryptedWallet, string encryptedSecretKey)
        {
            // TODO
            var plainTextWallet = new WalletContract();
            return Task.FromResult<WalletContract>(plainTextWallet);
        }

        public Task<CovidTestCredentialParameters> EncryptTestResults(CovidTestCredentialParameters plainTextResults, string encryptedSecretKey)
        {
            // TODO
            var encryptedResults = new CovidTestCredentialParameters();
            return Task.FromResult<CovidTestCredentialParameters>(encryptedResults);
        }

        public Task<CovidTestCredentialParameters> DecryptTestResults(CovidTestCredentialParameters encryptedResults, string encryptedSecretKey)
        {
            // TODO
            var plainTextResults = new CovidTestCredentialParameters();
            return Task.FromResult<CovidTestCredentialParameters>(plainTextResults);
        }
    }
}

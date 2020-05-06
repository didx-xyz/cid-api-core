using System;
using System.Threading.Tasks;
using CoviIDApiCore.V1.DTOs.Wallet;
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

        public Task<string> GenerateSecretKey ()
        {
            // TODO
            return Task.FromResult<string>("totally_secret_key");
        }

        public Task<WalletContract> EncryptWallet(WalletContract plainTextWallet, string secretKey)
        {
            // TODO
            var encryptedWallet = new WalletContract();
            return Task.FromResult<WalletContract>(encryptedWallet);
        }

        public Task<WalletContract> DecryptWallet(WalletContract encryptedWallet, string secretKey)
        {
            // TODO
            var plainTextWallet = new WalletContract();
            return Task.FromResult<WalletContract>(plainTextWallet);
        }
    }
}

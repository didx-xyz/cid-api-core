using System.Threading.Tasks;
using CoviIDApiCore.V1.DTOs.Wallet;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IEncryptionService
    {
        // TODO: All references to WalletContract below will change to reference the "new" version
        Task<string> GenerateSecretKey();
        Task<WalletContract> EncryptWallet(WalletContract plainTextWallet, string secretKey);
        // TODO: secretKey here should maybe be encryptedSecretKey
        Task<WalletContract> DecryptWallet(WalletContract encryptedWallet, string secretKey);
    }
}

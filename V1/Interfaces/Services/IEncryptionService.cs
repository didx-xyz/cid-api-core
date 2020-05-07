using System.Threading.Tasks;
// TODO: All references to WalletContract below will change to reference the "new" version
using CoviIDApiCore.V1.DTOs.Wallet;
// TODO: All references to CovidTestCredentialParameters below will change to reference the "new" version
using CoviIDApiCore.V1.DTOs.Credentials;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IEncryptionService
    {
        Task<string> GenerateEncryptedSecretKey();

        // These methods only really care about the mobile-number related
        // fields, but we accept the full wallet type for consistency.
        //
        // NOTE: These don't take in an encryptedSecretKey, because they are
        // encrypted using the serverKey only.
        Task<WalletContract> EncryptWallet(WalletContract plainTextWallet);
        Task<WalletContract> DecryptWallet(WalletContract encryptedWallet);

        Task<WalletContract> EncryptWalletDetails(WalletContract plainTextWallet, string encryptedSecretKey);
        Task<WalletContract> DecryptWalletDetails(WalletContract encryptedWallet, string encryptedSecretKey);

        Task<CovidTestCredentialParameters> EncryptTestResults(CovidTestCredentialParameters plainTextResults, string encryptedSecretKey);
        Task<CovidTestCredentialParameters> DecryptTestResults(CovidTestCredentialParameters encryptedResults, string encryptedSecretKey);
    }
}

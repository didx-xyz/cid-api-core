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

        Task<WalletContract> EncryptWalletDetails(WalletContract plainTextWallet, string encryptedSecretKey);
        Task<WalletContract> DecryptWalletDetails(WalletContract encryptedWallet, string encryptedSecretKey);

        Task<CovidTestCredentialParameters> EncryptTestResults(CovidTestCredentialParameters plainTextResults, string encryptedSecretKey);
        Task<CovidTestCredentialParameters> DecryptTestResults(CovidTestCredentialParameters encryptedResults, string encryptedSecretKey);
    }
}

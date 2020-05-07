using System.Threading.Tasks;
// TODO: All references to WalletContract below will change to reference the "new" version
using CoviIDApiCore.V1.DTOs.Wallet;
// TODO: All references to CovidTestCredentialParameters below will change to reference the "new" version
using CoviIDApiCore.V1.DTOs.Credentials;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface ICryptoService
    {
        Task<string> GenerateEncryptedSecretKey();

        void EncryptAsServer<T>(T obj);
        void DecryptAsServer<T>(T obj);

        void EncryptAsUser<T>(T obj, string encryptedSecretKey);
        void DecryptAsUser<T>(T obj, string encryptedSecretKey);
    }
}

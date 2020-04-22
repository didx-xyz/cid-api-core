using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.VerificationPolicy;
using CoviIDApiCore.V1.DTOs.Verifications;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Brokers
{
    public interface IAgencyBroker
    {
        #region Common
        Task<string> UploadFiles(string file, string fileName);
        #endregion

        #region Connections
        Task<ConnectionContract> CreateInvitation(ConnectionParameters connectionParameters);
        #endregion

        #region Credentials
        Task<CredentialsContract> SendCredentials(CredentialOfferParameters credentials);
        Task<CredentialsContract> GetCredential(string credentialId);
        #endregion

        #region Verifications
        Task<VerificationContract> SendVerification(VerificationPolicyParameters verificationPolicyParameters, string connectionId);
        Task<VerificationContract> GetVerification(string verificationId);
        #endregion
    }
}

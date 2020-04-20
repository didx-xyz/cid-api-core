using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Brokers
{
    public interface IAgencyBroker
    {
        #region Common
        Task<string> UploadFiles(string file, string fileName);
        #endregion

        #region Connections
        Task<ConnectionContract> CreateInvitation(ConnectionParameters connectionParameters, string agentId);
        #endregion
        #region Credentials
        Task<CredentialsContract> SendCredentials(CredentialOfferParameters credentials, string agent);
        Task<CredentialsContract> GetCredential(string credentialId, string agentId);
        #endregion

        #region Verifications
        //Task<List<VerificationContract>> GetVerifications(string connectionId);
        //Task<VerificationContract> GetVerification(string verificationId);
        //Task<VerificationResult> Verify(string verificationId);
        #endregion
    }
}

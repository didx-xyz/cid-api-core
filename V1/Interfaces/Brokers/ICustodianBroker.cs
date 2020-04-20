using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Brokers
{
    public interface ICustodianBroker
    {
        #region Wallet
        Task<List<WalletContract>> GetWallets(string agentId);
        Task<WalletContract> CreateWallet(WalletParameters walletParameters, string agentId);
        Task DeleteWallet(string walletId, string agentId);
        #endregion
        
        #region Connecitons
        Task<ConnectionContract> AcceptInvitation(string invitation, string walletId);
        #endregion

        #region Credentials
        Task AcceptCredential(string walletId, string credentialId);
        Task<List<CredentialsContract>> GetCredentialsByConnectionId(string walletId, string connectionId);
        Task<List<CredentialsContract>> GetCredentials(string walletId);
        #endregion
    }
}

using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IWalletService
    {
        Task<List<WalletContract>> GetWallets(string agentId);
        Task<WalletContract> CreateWallet(WalletParameters walletParameters, string agentId);
        Task<CoviIDWalletContract> CreateCoviIDWallet(CoviIDWalletParameters coviIDWalletParameters, string agentId);
        Task DeleteWallet(string walletId, string agentId);
        Task DeleteWallets(List<WalletParameters> wallets, string agentId);
    }
}

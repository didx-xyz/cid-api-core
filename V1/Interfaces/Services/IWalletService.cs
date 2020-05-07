using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Wallet;
using System;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IWalletService
    {
        Task<WalletStatusResponse> GetWalletStatus(Guid walletId, string key);
        Task<WalletResponse> CreateWallet(CreateWalletRequest walletRequest);
        Task<CoviIdWalletContract> CreateCoviIdWallet(CoviIdWalletParameters coviIdWalletParameters);
    }
}

using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Wallet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.DTOs.Authentication;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IWalletService
    {
        Task<WalletStatusResponse> GetWalletStatus(Guid walletId, string key);
        Task<WalletResponse> CreateWallet(CreateWalletRequest walletRequest);
        Task<CoviIdWalletContract> CreateCoviIdWallet(CoviIdWalletParameters coviIdWalletParameters);
        Task UpdateWallet(CovidTestCredentialParameters covidTest, string walletId);
        Task DeleteWallet(string walletId);
        Task DeleteWallets(List<WalletParameters> wallets);
    }
}

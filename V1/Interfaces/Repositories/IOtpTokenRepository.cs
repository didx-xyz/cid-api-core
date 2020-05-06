 using System.Collections.Generic;
 using System.Numerics;
 using System.Threading.Tasks;
 using CoviIDApiCore.Models.Database;

namespace CoviIDApiCore.V1.Interfaces.Repositories
{
    public interface IOtpTokenRepository : IBaseRepository<OtpToken, BigInteger>
    {
        Task<OtpToken> GetUnusedByMobileNumber(string mobileNumber);
        Task<OtpToken> GetBySessionId(string sessionId);
        Task<List<OtpToken>> GetAllUnexpiredByMobileNumberAsync(string mobileNumber);
//        Task<OtpToken> GetUnusedByWalletIdAndMobileNumber(string walletId, string mobileNumber);
    }
}

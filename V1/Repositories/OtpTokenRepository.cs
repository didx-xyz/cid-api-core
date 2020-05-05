using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using CoviIDApiCore.Data;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Interfaces.Repositories;

namespace CoviIDApiCore.V1.Repositories
{
    public class OtpTokenRepository : BaseRepository<OtpToken, long>, IOtpTokenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<OtpToken> _dbSet;

        public OtpTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.OtpTokens;
        }

        public async Task<OtpToken> GetUnusedByMobileNumber(string mobileNumber)
        {
            return await _dbSet
                .Where(t => string.Equals(t.MobileNumber, mobileNumber, StringComparison.Ordinal))
                .Where(t => !t.isUsed)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<OtpToken> GetUnusedByWalletIdAndMobileNumber(string walletId, string mobileNumber)
        {
            return await _dbSet
                .Where(t => string.Equals(t.Wallet.WalletIdentifier, walletId, StringComparison.Ordinal))
                .Where(t => string.Equals(t.MobileNumber, mobileNumber, StringComparison.Ordinal))
                .Where(t => !t.isUsed)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}

using CoviIDApiCore.Data;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoviIDApiCore.V1.Repositories
{
    public class WalletTestResultRepository : BaseRepository<WalletTestResult, Guid>, IWalletTestResultRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<WalletTestResult> _dbset;

        public WalletTestResultRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbset = _context.WalletTestResults;
        }

    }
}

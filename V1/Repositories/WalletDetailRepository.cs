using CoviIDApiCore.Data;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoviIDApiCore.V1.Repositories
{
    public class WalletDetailRepository : BaseRepository<WalletDetail, Guid>, IWalletDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<WalletDetail> _dbset;

        public WalletDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbset = _context.WalletDetails;
        }

    }
}

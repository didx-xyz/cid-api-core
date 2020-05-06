using CoviIDApiCore.Data;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

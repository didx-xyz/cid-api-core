﻿using Microsoft.EntityFrameworkCore;
using System;
 using System.Linq;
 using System.Threading.Tasks;
 using CoviIDApiCore.Data;
 using CoviIDApiCore.Models.Database;
 using CoviIDApiCore.V1.Interfaces.Repositories;

 namespace CoviIDApiCore.V1.Repositories
{
    public class TokenRepository : BaseRepository<Token, long>, ITokenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Token> _dbSet;

        public TokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Tokens;
        }

        public async Task<Token> GetUnusedByMobileNumber(string mobileNumber)
        {
            return await _dbSet
                .Where(t => string.Equals(t.MobileNumber, mobileNumber, StringComparison.Ordinal))
                .Where(t => !t.isUsed)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}

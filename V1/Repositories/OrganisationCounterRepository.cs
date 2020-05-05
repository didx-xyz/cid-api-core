using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using CoviIDApiCore.Data;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Interfaces.Repositories;

namespace CoviIDApiCore.V1.Repositories
{
    public class OrganisationCounterRepository : BaseRepository<OrganisationCounter, Guid>, IOrganisationCounterRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<OrganisationCounter> _dbSet;

        public OrganisationCounterRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.OrganisationCounters;
        }

        public async Task<OrganisationCounter> GetLastByOrganisation(Organisation organisation)
        {
            return await _dbSet
                .Where(t => t.Organisation == organisation)
                .Where(t => t.Date.Date == DateTime.UtcNow.Date)
                .OrderByDescending(t => t.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CountToday(Organisation organisation)
        {
            return await _dbSet.CountAsync(t => t.Organisation == organisation && t.Date.Date == DateTime.UtcNow.Date);
        }
    }
}

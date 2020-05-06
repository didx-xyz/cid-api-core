using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using CoviIDApiCore.Data;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Interfaces.Repositories;

namespace CoviIDApiCore.V1.Repositories
{
    public class OrganisationAccessLogRepository : BaseRepository<OrganisationAccessLog, Guid>, IOrganisationAccessLogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<OrganisationAccessLog> _dbSet;

        public OrganisationAccessLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.OrganisationAccessLogs;
        }

        public async Task<OrganisationAccessLog> GetLastByOrganisation(Organisation organisation)
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

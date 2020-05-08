﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<List<OrganisationAccessLog>> GetAllCurrentDayByOrganisation(Organisation organisation)
        {
            return await _dbSet
                .Where(oal => oal.Organisation == organisation)
                .Where(aol => aol.CreatedAt.Date == DateTime.UtcNow.Date)
                .ToListAsync();
        }
    }
}
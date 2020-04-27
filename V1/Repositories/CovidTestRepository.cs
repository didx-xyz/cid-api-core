using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Repositories
{
    public class CovidTestRepository : BaseRepository<CovidTest, Guid>, ICovidTestRepository
    {
    }
}

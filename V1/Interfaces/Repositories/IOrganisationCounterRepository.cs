 using System;
 using System.Threading.Tasks;
 using CoviIDApiCore.Models.Database;

namespace CoviIDApiCore.V1.Interfaces.Repositories
{
    public interface IOrganisationAccessLogRepository : IBaseRepository<OrganisationAccessLog, Guid>
    {
        Task<OrganisationAccessLog> GetLastByOrganisation(Organisation organisation);
        Task<int> CountToday(Organisation organisation);
    }
}

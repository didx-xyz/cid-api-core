 using System.Threading.Tasks;
 using CoviIDApiCore.Models.Database;

namespace CoviIDApiCore.V1.Interfaces.Repositories
{
    public interface ITokenRepository : IBaseRepository<Token, long>
    {
        Task<Token> GetUnusedByMobileNumber(string mobileNumber);
    }
}

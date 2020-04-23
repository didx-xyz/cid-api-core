using CoviIDApiCore.V1.DTOs.Verify;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IVerifyService
    {
        Task<VerifyResult> GetCredentials(string walletId, string organisationId, string deviceIdentifier);
    }
}

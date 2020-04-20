using CoviIDApiCore.V1.DTOs.Verify;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IVerifyService
    {
        Task<VerifyResult> VerifyCredentials(string walletId);
    }
}

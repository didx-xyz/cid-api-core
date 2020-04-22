using CoviIDApiCore.V1.DTOs.Connection;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IConnectionService
    {
        Task<ConnectionContract> CreateInvitation(ConnectionParameters connectionParameters);
        Task<ConnectionContract> AcceptInvitation(string invitation, string walletId);
    }
}

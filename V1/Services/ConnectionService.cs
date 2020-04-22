using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Services;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly IAgencyBroker _agencyBroker;
        private readonly ICustodianBroker _custodianBroker;

        public ConnectionService(IAgencyBroker agencyBroker, ICustodianBroker custodianBroker)
        {
            _agencyBroker = agencyBroker;
            _custodianBroker = custodianBroker;
        }

        public async Task<ConnectionContract> CreateInvitation(ConnectionParameters connectionParameters)
        {
            return await _agencyBroker.CreateInvitation(connectionParameters);
        }
        public async Task<ConnectionContract> AcceptInvitation(string invitation, string walletId)
        {
            return await _custodianBroker.AcceptInvitation(invitation, walletId);
        }
    }
}

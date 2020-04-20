using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Verify;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Services
{
    public class VerifyService : IVerifyService
    {
        private readonly ICustodianBroker _custodianBroker;

        public VerifyService(ICustodianBroker custodianBroker)
        {
            _custodianBroker = custodianBroker;
        }

        public async Task<VerifyResult> VerifyCredentials(string walletId)
        {
            var credentialList = await _custodianBroker.GetCredentials(walletId);

            // todo get where it was the last test taken
            var credentials = credentialList.FirstOrDefault(x => x.State == CredentialsState.Requested);

            if (credentials == null)
                return null;

            //TODO: Optimize
            credentials.Values.TryGetValue("Picture", out string picture);
            credentials.Values.TryGetValue("Name", out string name);
            credentials.Values.TryGetValue("Surname", out string surname);
            credentials.Values.TryGetValue("CovidStatus", out string covidStatus);

            var enumValue = (int)Enum.Parse(typeof(CovidStatus), covidStatus);

            return new VerifyResult
            {
                Picture = picture,
                Name = name,
                Surname = surname,
                Status = enumValue,
                CovidStatus = covidStatus
            };
        }
    }
}

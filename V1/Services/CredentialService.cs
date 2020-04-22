using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CoviIDApiCore.V1.Constants.DefinitionConstants;

namespace CoviIDApiCore.V1.Services
{
    /// <summary>
    /// This class ensures that the credentials are always created with the correct values and associated Definition ID. 
    /// This has context of the correct Definition ID and does not require a lookup for the ID's.
    /// </summary>
    public class CredentialService : ICredentialService
    {
        private readonly IAgencyBroker _agencyBroker;
        public CredentialService(IAgencyBroker agencyBroker)
        {
            _agencyBroker = agencyBroker;
        }
        
        /// <summary>
        /// Creates a credentials with the relevant DefinitionID and attribute values.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="personalDetials"></param>
        /// <returns></returns>
        public async Task<CredentialsContract> CreatePersonalDetials(string connectionId, PersonalDetialsCredentialParameters personalDetials)
        {
            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = connectionId,
                DefinitionId = DefinitionIds[Schemas.PersonalDetials],
                AutomaticIssuance = false,
                CredentialValues = new Dictionary<string, string>
                {
                    { "Name" , personalDetials.Name },
                    { "Surname" , personalDetials.Surname },
                    { "Picture" , personalDetials.Picture },
                    { "TelNumber" , personalDetials.TelNumber },
                }
            };

            var credentials = await _agencyBroker.SendCredentials(credentialOffer);
            return credentials;
        }
       
        public async Task<CredentialsContract> CreateCovidTest(string connectionId, CovidTestCredentialParameters covidTestCredential)
        {
            // TODO add a null check
            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = connectionId,
                DefinitionId = DefinitionIds[Schemas.CovidTest],
                AutomaticIssuance = false,
                CredentialValues = new Dictionary<string, string>
                {
                    { "ReferenceNumber" , covidTestCredential.ReferenceNumber },
                    { "Laboratory" , covidTestCredential.Labratory.ToString() },
                    { "TestDate" , covidTestCredential.TestDate.ToString() },
                    { "ExpiryDate" , covidTestCredential.ExpiryDate.ToString() },
                    { "Status" , covidTestCredential.CovidStatus.ToString() },
                }
            };

            var credentials = await _agencyBroker.SendCredentials(credentialOffer);
            return credentials;
        }

        public async Task<CredentialsContract> CreateIdentification(string connectionId, IdentificationCredentialParameter identification)
        {
            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = connectionId,
                DefinitionId = DefinitionIds[Schemas.Identification],
                AutomaticIssuance = false,
                CredentialValues = new Dictionary<string, string>
                {
                    { "Type" , identification.IdentificationType.ToString() },
                    { "Value" , identification.Identification }
                }
            };

            var credentials = await _agencyBroker.SendCredentials(credentialOffer);
            return credentials;
        }
    }
}

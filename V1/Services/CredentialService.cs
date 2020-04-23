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
        /// Creates a verified person credentials with the relevant DefinitionID and attribute values.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="personCredential"></param>
        /// <returns></returns>
        public async Task<CredentialsContract> CreatePerson(string connectionId, PersonCredentialParameters personCredential)
        {
            // TODO : validate data
            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = connectionId,
                DefinitionId = DefinitionIds[Schemas.Personal],
                AutomaticIssuance = false,
                CredentialValues = new Dictionary<string, string>
                {
                    { Attributes.FirstName , personCredential.FirstName },
                    { Attributes.LastName, personCredential.LastName },
                    { Attributes.PhotoUrl, personCredential.Photo },
                    { Attributes.MobileNumber , personCredential.MobileNumber.ToString() },
                    { Attributes.IdentificationType , personCredential.IdentificationType.ToString() },
                    { Attributes.IdentificationValue, personCredential.IdentificationValue.ToString() }
                }
            };

            var credentials = await _agencyBroker.SendCredentials(credentialOffer);
            return credentials;
        }
       
        public async Task<CredentialsContract> CreateCovidTest(string connectionId, CovidTestCredentialParameters covidTestCredential)
        {
            // TODO : validate data
            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = connectionId,
                DefinitionId = DefinitionIds[Schemas.CovidTest],
                AutomaticIssuance = false,
                CredentialValues = new Dictionary<string, string>
                {
                    { Attributes.ReferenceNumber , covidTestCredential.ReferenceNumber },
                    { Attributes.Laboratory , covidTestCredential.Laboratory.ToString() },
                    { Attributes.DateTested , covidTestCredential.DateTested.ToString() },
                    { Attributes.DateIssued, covidTestCredential.DateIssued.ToString() },
                    { Attributes.CovidStatus, covidTestCredential.CovidStatus.ToString() },
                }
            };

            var credentials = await _agencyBroker.SendCredentials(credentialOffer);
            return credentials;
        }
    }
}

using CoviIDApiCore.Helpers;
using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICustodianBroker _custodianBroker;
        private readonly IConnectionService _connectionService;

        public CredentialService(IAgencyBroker agencyBroker, ICustodianBroker custodianBroker, IConnectionService connectionService)
        {
            _agencyBroker = agencyBroker;
            _custodianBroker = custodianBroker;
            _connectionService = connectionService;
        }
        
        /// <summary>
        /// Creates a verified person credentials with the relevant DefinitionID and attribute values.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="personCredential"></param>
        /// <returns></returns>
        public async Task<CredentialsContract> CreatePerson(string connectionId, PersonCredentialParameters personCredential)
        {
            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = connectionId,
                DefinitionId = DefinitionIds[Schemas.Person],
                AutomaticIssuance = false,
                CredentialValues = new Dictionary<string, string>
                {
                    { Attributes.FirstName , personCredential.FirstName.ValidateLength() },
                    { Attributes.LastName, personCredential.LastName.ValidateLength() },
                    { Attributes.Photo, personCredential.Photo },
                    { Attributes.MobileNumber , personCredential.MobileNumber.ValidateMobileNumber().ToString() },
                    { Attributes.IdentificationType , personCredential.IdentificationType.ToString() },
                    { Attributes.IdentificationValue, personCredential.IdentificationValue.ValidateIdentification(personCredential.IdentificationType) }
                }
            };

            var credentials = await _agencyBroker.SendCredentials(credentialOffer);
            return credentials;
        }
       
        public async Task<CredentialsContract> CreateCovidTest(string connectionId, CovidTestCredentialParameters covidTestCredential)
        {
            covidTestCredential.DateIssued = DateTime.UtcNow;
            var credentialOffer = new CredentialOfferParameters
            {
                ConnectionId = connectionId,
                DefinitionId = DefinitionIds[Schemas.CovidTest],
                AutomaticIssuance = false,
                CredentialValues = new Dictionary<string, string>
                {
                    { Attributes.ReferenceNumber , covidTestCredential.ReferenceNumber.ValidateLength() },
                    { Attributes.Laboratory , covidTestCredential.Laboratory.ToString() },
                    { Attributes.DateTested , covidTestCredential.DateTested.ValidateIsInPast().ToString() },
                    { Attributes.DateIssued, covidTestCredential.DateIssued.ToString() },
                    { Attributes.CovidStatus, covidTestCredential.CovidStatus.ToString() },
                }
            };

            var credentials = await _agencyBroker.SendCredentials(credentialOffer);
            return credentials;
        }
        public async Task CreatePersonAndCovidTestCredentials(CovidTestCredentialParameters covidTest, PersonCredentialParameters person, string walletId)
        {
            var connectionParameters = new ConnectionParameters
            {
                ConnectionId = "", // Leave blank for auto generation
                Multiparty = false,
                Name = "CoviID", // This is the Agent name
            };

            var agentInvitation = await _connectionService.CreateInvitation(connectionParameters);
            var custodianConnection = await _connectionService.AcceptInvitation(agentInvitation.Invitation, walletId);

            // Create the set of credentials
            var personalDetialsCredentials = await CreatePerson(agentInvitation.ConnectionId, person);

            if (covidTest != null)
            {
                var covidTestCredentials = await CreateCovidTest(agentInvitation.ConnectionId, covidTest);
            }

            var userCredentials = await _custodianBroker.GetCredentials(walletId);
            var offeredCredentials = userCredentials.Where(x => x.State == CredentialsState.Offered);

            if (offeredCredentials != null)
            {
                // Accept all the credentials
                foreach (var offer in offeredCredentials)
                {
                    await _custodianBroker.AcceptCredential(walletId, offer.CredentialId);
                }
            }
            return;
        }
    }
}

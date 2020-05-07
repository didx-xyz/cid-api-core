using CoviIDApiCore.Helpers;
using CoviIDApiCore.Models.Database;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Repositories;
using CoviIDApiCore.V1.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly ICovidTestRepository _covidTestRepository;

        public CredentialService(IAgencyBroker agencyBroker, ICustodianBroker custodianBroker, IConnectionService connectionService,
            ICovidTestRepository covidTestRepository)
        {
            _agencyBroker = agencyBroker;
            _custodianBroker = custodianBroker;
            _connectionService = connectionService;
            _covidTestRepository = covidTestRepository;
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

        public async Task<CredentialsContract> CreateCovidTest(string connectionId, CovidTestCredentialParameters covidTestCredential, string walletId)
        {
            CredentialsContract credentials = null;
            if (covidTestCredential != null)
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
                        { Attributes.DateTested , covidTestCredential.DateTested.IsInPast().ToString() },
                        { Attributes.DateIssued, covidTestCredential.DateIssued.ToString() },
                        { Attributes.CovidStatus, covidTestCredential.CovidStatus.ToString() },
                    }
                };

                credentials = await _agencyBroker.SendCredentials(credentialOffer);
                //StoreCoviIDCredentials(covidTestCredential, walletId);
            }
            return credentials;
        }

        public async Task CreatePersonAndCovidTestCredentials(CovidTestCredentialParameters covidTest, PersonCredentialParameters person, string walletId)
        {
            var connectionParameters = new ConnectionParameters
            {
                ConnectionId = "", // Leave blank for auto generation
                Multiparty = false,
                Name = AgentName
            };

            var agentInvitation = await _connectionService.CreateInvitation(connectionParameters);
            var custodianConnection = await _connectionService.AcceptInvitation(agentInvitation.Invitation, walletId);

            // Create the set of credentials
            var personalDetialsCredentials = await CreatePerson(agentInvitation.ConnectionId, person);
            var covidTestCredentials = await CreateCovidTest(agentInvitation.ConnectionId, covidTest, walletId);

            var userCredentials = await _custodianBroker.GetCredentials(walletId);
            if (userCredentials == null)
                throw new ValidationException(Messages.Cred_NotFound);

            var offeredCredentials = userCredentials?.Where(x => x.State == CredentialsState.Offered);
            if (offeredCredentials == null)
                throw new ValidationException(Messages.Cred_OfferedNotFound);

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

        public async Task<CoviIDCredentialContract> GetCoviIDCredentials(string walletId)
        {
            var covidTestCredentials = new CovidTestCredentialParameters();

            var allCredentials = await _custodianBroker.GetCredentials(walletId);
            if (allCredentials == null)
                throw new ValidationException(Messages.Cred_NotFound);

            var offeredCredentials = allCredentials?.Where(x => x.State == CredentialsState.Requested).ToList();
            if (offeredCredentials == null)
                throw new ValidationException(Messages.Cred_RequestedNotFound);


            var verifiedPerson = offeredCredentials?.FirstOrDefault(p => p.DefinitionId == DefinitionIds[Schemas.Person]);
            var covidTest = offeredCredentials?.Where(c => c.DefinitionId == DefinitionIds[Schemas.CovidTest])
                .OrderBy(c => c.Values.TryGetValue(Attributes.DateIssued, out var dateIssued)).ToList().FirstOrDefault();

            if (verifiedPerson != null)
            {
                //TODO: Optimize
                verifiedPerson.Values.TryGetValue(Attributes.FirstName, out string firstName);
                verifiedPerson.Values.TryGetValue(Attributes.LastName, out string lastName);
                verifiedPerson.Values.TryGetValue(Attributes.Photo, out string photo);
                verifiedPerson.Values.TryGetValue(Attributes.MobileNumber, out string mobileNumber);
                verifiedPerson.Values.TryGetValue(Attributes.IdentificationType, out string identificationTypeStr);
                verifiedPerson.Values.TryGetValue(Attributes.IdentificationValue, out string identificationValue);

                var identificationType = (IdentificationTypes)Enum.Parse(typeof(IdentificationTypes), identificationTypeStr);

                if (covidTest != null)
                {
                    covidTest.Values.TryGetValue(Attributes.ReferenceNumber, out string referenceNumber);
                    covidTest.Values.TryGetValue(Attributes.Laboratory, out string laboratoryStr);
                    covidTest.Values.TryGetValue(Attributes.DateTested, out string dateTested);
                    covidTest.Values.TryGetValue(Attributes.DateIssued, out string dateIssued);
                    covidTest.Values.TryGetValue(Attributes.CovidStatus, out string covidStatusStr);


                    var laboratory = (Laboratory)Enum.Parse(typeof(Laboratory), laboratoryStr);
                    var covidStatus = (CovidStatus)Enum.Parse(typeof(CovidStatus), covidStatusStr);

                    covidTestCredentials.DateIssued = DateTime.Parse(dateIssued);
                    covidTestCredentials.DateTested = DateTime.Parse(dateTested);
                    covidTestCredentials.Laboratory = laboratory;
                    covidTestCredentials.ReferenceNumber = referenceNumber;
                    covidTestCredentials.CovidStatus = covidStatus;
                }

                return new CoviIDCredentialContract
                {
                    CovidTestCredentials = covidTestCredentials,
                    PersonCredentials = new PersonCredentialParameters
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        IdentificationType = identificationType,
                        IdentificationValue = identificationValue,
                        MobileNumber = long.Parse(mobileNumber),
                        Photo = photo
                    }
                };
            }
            throw new ValidationException(Messages.Cred_VerifidPersonNotFound);
        }
        //private async void StoreCoviIDCredentials(CovidTestCredentialParameters covidTestParameters, string walletId)
        //{
        //    if (covidTestParameters.HasConsent)
        //    {
        //        var covidTest = new WalletTestResult
        //        {
        //            ResultStatus = covidTestParameters.CovidStatus,
        //            IssuedAt = covidTestParameters.DateIssued,
        //            TestedAt = covidTestParameters.DateTested,
        //            HasConsent = covidTestParameters.HasConsent,
        //            Laboratory = covidTestParameters.Laboratory,
        //            PermissionGrantedAt = DateTime.Now,
        //            ReferenceNumber = covidTestParameters.ReferenceNumber,
        //            //WalletId = walletId,
        //            LaboratoryStatus = LaboratoryStatus.Unsent
        //        };
        //        await _covidTestRepository.AddAsync(covidTest);
        //        await _covidTestRepository.SaveAsync();

        //    }
        //    return;
        //}
    }
}

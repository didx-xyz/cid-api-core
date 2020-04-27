﻿using CoviIDApiCore.Exceptions;
using CoviIDApiCore.Helpers;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Wallet;
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
        public CredentialService(IAgencyBroker agencyBroker, ICustodianBroker custodianBroker)
        {
            _agencyBroker = agencyBroker;
            _custodianBroker = custodianBroker;
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

        public async Task<CoviIDCredentialContract> GetCoviIDCredentials(string walletId)
        {
            var allCredentials = await _custodianBroker.GetCredentials(walletId);
            var offeredCredentials = allCredentials.Where(x => x.State == CredentialsState.Requested).ToList();

            var verifiedPerson = offeredCredentials.FirstOrDefault(p => p.DefinitionId == DefinitionIds[Schemas.Person]);
            var covidTest = offeredCredentials.FirstOrDefault(p => p.DefinitionId == DefinitionIds[Schemas.CovidTest]);

            if (verifiedPerson != null)
            {
                //TODO: Optimize
                verifiedPerson.Values.TryGetValue(Attributes.FirstName, out string firstName);
                verifiedPerson.Values.TryGetValue(Attributes.LastName, out string lastName);
                verifiedPerson.Values.TryGetValue(Attributes.Photo, out string photo);
                verifiedPerson.Values.TryGetValue(Attributes.MobileNumber, out string mobileNumber);
                verifiedPerson.Values.TryGetValue(Attributes.IdentificationType, out string identificationTypeStr);
                verifiedPerson.Values.TryGetValue(Attributes.IdentificationValue, out string identificationValue);

                covidTest.Values.TryGetValue(Attributes.ReferenceNumber, out string referenceNumber);
                covidTest.Values.TryGetValue(Attributes.Laboratory, out string laboratoryStr);
                covidTest.Values.TryGetValue(Attributes.DateTested, out string dateTested);
                covidTest.Values.TryGetValue(Attributes.DateIssued, out string dateIssued);
                covidTest.Values.TryGetValue(Attributes.CovidStatus, out string covidStatusStr);


                var laboratory = (Laboratory)Enum.Parse(typeof(Laboratory), laboratoryStr);
                var identificationTypes = (IdentificationTypes)Enum.Parse(typeof(IdentificationTypes), identificationTypeStr);
                var covidStatus = (CovidStatus)Enum.Parse(typeof(CovidStatus), covidStatusStr);


                return new CoviIDCredentialContract
                {
                    CovidTestCredentials = new CovidTestCredentialParameters
                    {
                        DateIssued = DateTime.Parse(dateIssued),
                        DateTested = DateTime.Parse(dateTested),
                        Laboratory = laboratory,
                        ReferenceNumber = referenceNumber,
                        CovidStatus = covidStatus,
                    },
                    PersonCredentials = new PersonCredentialParameters
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        IdentificationType = identificationTypes,
                        IdentificationValue = identificationValue,
                        MobileNumber = long.Parse(mobileNumber),
                        Photo = photo
                    }
                };
            }
            //TODO : throw exception/handle
            return null;
        }
    }
}

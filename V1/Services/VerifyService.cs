using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.VerificationPolicy;
using CoviIDApiCore.V1.DTOs.Verifications;
using CoviIDApiCore.V1.DTOs.Verify;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;

namespace CoviIDApiCore.V1.Services
{
    public class VerifyService : IVerifyService
    {
        private readonly ICustodianBroker _custodianBroker;
        private readonly IAgencyBroker _agencyBroker;
        private readonly IOrganisationService _organisationService;
        private readonly ICredentialService _credentialService;

        public VerifyService(ICustodianBroker custodianBroker, IAgencyBroker agencyBroker, IOrganisationService organisationService, ICredentialService credentialService)
        {
            _custodianBroker = custodianBroker;
            _agencyBroker = agencyBroker;
            _organisationService = organisationService;
            _credentialService = credentialService;
        }

        public async Task<VerifyResult> GetCredentials(string walletId, string organisationId, string deviceIdentifier)
        {
            var coviIdCredentials = await _credentialService.GetCoviIDCredentials(walletId);

            if(coviIdCredentials == default)
                throw new NotFoundException();
            
            if (!string.IsNullOrEmpty(organisationId))
                await _organisationService.UpdateCountAsync(organisationId, deviceIdentifier, UpdateType.Addition);

            return new VerifyResult
            {
                Picture = coviIdCredentials.PersonCredentials.Photo,
                Name = coviIdCredentials.PersonCredentials.FirstName,
                Surname = coviIdCredentials.PersonCredentials.LastName,
                Status = (int)coviIdCredentials.CovidTestCredentials.CovidStatus,
                CovidStatus = coviIdCredentials.CovidTestCredentials.CovidStatus.ToString()
            };
        }

        public async Task VerifyCredentials(string walletId, string connectionId, string custodianConnectionId)
        {
            var verificationPolicyParameters = new VerificationPolicyParameters
            {
                Name = "CoviID",
                Version = "1.0",
                Attributes = new VerificationPolicyAttributeContract
                {
                    PolicyName = "CoviID Policy",
                    AttributeNames = new List<string> {
                        "Name",
                        "Surname",
                        "Id",
                        "Picture",
                        "CovidStatue",
                        "TestDate",
                        "ExpiryDate"
                    }
                },
                RevocationRequirement = new RevocationRequirement
                {
                    ValidAt = DateTime.Now.AddMonths(1)
                }
            };

            // 1. issue verification
            var verificationContract = await _agencyBroker.SendVerification(verificationPolicyParameters, connectionId);

            // 3. Lookup Verification id
            var userVerificationsContract = await _custodianBroker.GetVerifications(walletId, custodianConnectionId);
            var verificationItem = userVerificationsContract?.FirstOrDefault(v => v.State == ProofState.Requested);
            if (verificationItem == null)
            {
                //todo throw error
            }

            // 2. user accepts verification
            await _custodianBroker.AcceptVerification(walletId, verificationItem.VerificationId);

            var verifications = await _agencyBroker.GetVerification(verificationContract.VerificationId);

            // 4. verify agains ledger (get verification)
        }

    }
}

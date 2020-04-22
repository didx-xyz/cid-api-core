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

namespace CoviIDApiCore.V1.Services
{
    public class VerifyService : IVerifyService
    {
        private readonly ICustodianBroker _custodianBroker;
        private readonly IAgencyBroker _agencyBroker;

        public VerifyService(ICustodianBroker custodianBroker, IAgencyBroker agencyBroker)
        {
            _custodianBroker = custodianBroker;
            _agencyBroker = agencyBroker;
        }

        public async Task<VerifyResult> GetCredentials(string walletId)
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

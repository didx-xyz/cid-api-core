using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoviIDApiCore.V1.DTOs.VerificationPolicy
{
    public class VerificationPolicyAttributeContract
    {
        [JsonProperty("policyName")]
        public string PolicyName { get; set; }

        [JsonProperty("attributeNames")]
        public List<string> AttributeNames { get; set; }

        [JsonProperty("restrictions")]
        public List<VerificationPolicyRestriction> Restrictions { get; set; }
    }
}

using Newtonsoft.Json;

namespace CoviIDApiCore.V1.DTOs.VerificationPolicy
{
    public class VerificationPolicyPredicateContract
    {
        [JsonProperty("policyName")]
        public string PolicyName { get; set; }

        [JsonProperty("attributeName")]
        public string AttributeName { get; set; }

        [JsonProperty("predicateType")]
        public string PredicateType { get; set; }

        [JsonProperty("predicateValue")]
        public long PredicateValue { get; set; }

        [JsonProperty("restrictions")]
        public VerificationPolicyRestriction Restrictions { get; set; }
    }
}

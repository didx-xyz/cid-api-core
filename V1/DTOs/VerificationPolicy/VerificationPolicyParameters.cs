using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoviIDApiCore.V1.DTOs.VerificationPolicy
{
    public class VerificationPolicyParameters
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("attributes")]
        public VerificationPolicyAttributeContract Attributes { get; set; }

        [JsonProperty("predicates")]
        public List<VerificationPolicyPredicateContract> Predicates { get; set; }

        [JsonProperty("revocationRequirement")]
        public RevocationRequirement RevocationRequirement { get; set; }
    }
}

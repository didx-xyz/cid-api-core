using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoviIDApiCore.V1.DTOs.Verifications
{
    public class ProofAttributeContract
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("attributes")]
        public Dictionary<string, string> Attributes { get; set; }

        [JsonProperty("revealed")]
        public bool Revealed { get; set; }

        [JsonProperty("selfAttested")]
        public bool SelfAttested { get; set; }

        [JsonProperty("conditional")]
        public bool Conditional { get; set; }
    }
}

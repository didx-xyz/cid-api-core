using Newtonsoft.Json;

namespace CoviIDApiCore.V1.DTOs.VerificationPolicy
{
    public class VerificationPolicyRestriction
    {

        [JsonProperty("schemaId")]
        public string SchemaId { get; set; }

        [JsonProperty("schemaIssuerDid")]
        public string SchemaIssuerDid { get; set; }

        [JsonProperty("schemaName")]
        public string SchemaName { get; set; }

        [JsonProperty("schemaVersion")]
        public string SchemaVersion { get; set; }

        [JsonProperty("issuerDid")]
        public string IssuerDid { get; set; }

        [JsonProperty("credentialDefinitionId")]
        public string CredentialDefinitionId { get; set; }

        [JsonProperty("value")]
        public VerificationPolicyRestrictionAttribute Value { get; set; }
    }
}

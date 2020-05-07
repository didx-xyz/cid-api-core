using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CoviIDApiCore.V1.DTOs.Verifications
{
    public class VerificationContract
    {
        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty("verificationId")]
        public string VerificationId { get; set; }

        [JsonProperty("definitionId")]
        public string DefinitionId { get; set; }

        [JsonProperty("state")]
        public ProofState State { get; set; }

        [JsonProperty("createdAtUtc")]
        public DateTimeOffset CreatedAtUtc { get; set; }

        [JsonProperty("updatedAtUtc")]
        public DateTimeOffset UpdatedAtUtc { get; set; }

        [JsonProperty("isValid")]
        public bool IsValid { get; set; }

        [JsonProperty("verifiedAtUtc")]
        public DateTimeOffset VerifiedAtUtc { get; set; }

        [JsonProperty("proof")]
        public List<ProofAttributeContract> Proof { get; set; }

        [JsonProperty("verificationRequestData")]
        public string VerificationRequestData { get; set; }

        [JsonProperty("verificationRequestUrl")]
        public string VerificationRequestUrl { get; set; }
    }
}

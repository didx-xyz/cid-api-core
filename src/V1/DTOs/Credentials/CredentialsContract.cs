using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CredentialsContract
    {
        [JsonProperty("credentialId")]
        public string CredentialId { get; set; }

        [JsonProperty("state")]
        public CredentialsState State { get; set; }

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty("definitionId")]
        public string DefinitionId { get; set; }

        [JsonProperty("schemaId")]
        public string SchemaId { get; set; }

        [JsonProperty("offerData")]
        public string OfferData { get; set; }

        [JsonProperty("offerUrl")]
        public string OfferUrl { get; set; }

        [JsonProperty("issuedAtUtc")]
        public DateTimeOffset IssuedAtUtc { get; set; }

        [JsonProperty("acceptedAtUtc")]
        public DateTimeOffset AcceptedAtUtc { get; set; }

        [JsonProperty("values")]
        public Dictionary<string, string> Values { get; set; }
    }
}

using CoviIDApiCore.V1.DTOs.Agent;
using Newtonsoft.Json;
using System;

namespace CoviIDApiCore.V1.DTOs.Connection
{
    public class ConnectionContract
    {
        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("myDid")]
        public string MyDid { get; set; }

        [JsonProperty("theirDid")]
        public string TheirDid { get; set; }

        [JsonProperty("myKey")]
        public string MyKey { get; set; }

        [JsonProperty("theirKey")]
        public string TheirKey { get; set; }

        [JsonProperty("state")]
        public ConnectionStatus State { get; set; }

        [JsonProperty("invitation")]
        public string Invitation { get; set; }

        [JsonProperty("invitationUrl")]
        public string InvitationUrl { get; set; }

        [JsonProperty("endpoint")]
        public AgentEndpoint Endpoint { get; set; }

        [JsonProperty("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; }

        [JsonProperty("multiParty")]
        public bool MultiParty { get; set; }
    }
}


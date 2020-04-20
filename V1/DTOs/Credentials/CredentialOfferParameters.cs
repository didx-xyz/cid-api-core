using System.Collections.Generic;

namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CredentialOfferParameters
    {
        public string DefinitionId { get; set; }
        public string ConnectionId { get; set; }
        public bool AutomaticIssance { get; set; }
        public Dictionary<string, string> CredentialValues { get; set; }
    }
}

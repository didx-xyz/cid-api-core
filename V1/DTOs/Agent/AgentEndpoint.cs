using System.Collections.Generic;

namespace CoviIDApiCore.V1.DTOs.Agent
{
    public class AgentEndpoint
    {
        public string Did { get; set; }
        public List<string> VerKey { get; set; }
        public string Uri { get; set; }
    }
}

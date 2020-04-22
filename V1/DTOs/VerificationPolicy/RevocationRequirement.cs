using Newtonsoft.Json;
using System;

namespace CoviIDApiCore.V1.DTOs.VerificationPolicy
{
    public class RevocationRequirement
    {
        [JsonProperty("validAt")]
        public DateTimeOffset ValidAt { get; set; }
    }
}

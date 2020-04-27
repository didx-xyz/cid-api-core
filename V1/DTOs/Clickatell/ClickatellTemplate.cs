using System;
using Newtonsoft.Json;

namespace CoviIDApiCore.V1.DTOs.Clickatell
{
    public class ClickatellTemplate
    {
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("to")] public string[] To { get; set; }
    }
}
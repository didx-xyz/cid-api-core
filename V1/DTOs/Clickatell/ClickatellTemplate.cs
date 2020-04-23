using Newtonsoft.Json;

namespace CoviIDApiCore.V1.DTOs.Clickatell
{
    public class ClickatellTemplate
    {
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("to")] public string[] To { get; set; }
        [JsonProperty("from")] public string From { get; set; }
        [JsonProperty("binary")] public bool Binary { get; set; } = false;
        [JsonProperty("clientMessageId")] public string ClientMessageId { get; set; }
        [JsonProperty("scheduledDeliveryTime")] public string ScheduledDeliveryTime { get; set; }
        [JsonProperty("userDataHeader")] public string UserDataHeader { get; set; }
        [JsonProperty("validityPeriod")] public int ValidityPeriod { get; set; }
        [JsonProperty("charset")] public string CharSet { get; set; }
    }
}
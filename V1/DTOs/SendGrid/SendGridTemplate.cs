using Newtonsoft.Json;

namespace CoviIDApiCore.V1.DTOs.SendGrid
{
    public class SendGridTemplate
    {
        [JsonProperty("personalizations")] public Personalizations[] Personalizations { get; set; }
        [JsonProperty("from")] public SentFrom From { get; set; }
        [JsonProperty("attachments")] public Attachment[] Attachments { get; set; }
        [JsonProperty("template_Id")] public string TemplateId { get; set; }
    }

    public class Personalizations
    {
        [JsonProperty("to")] public SendTo[] To { get; set; }

        [JsonProperty("subject")] public string Subject { get; set; }

        [JsonProperty("dynamic_template_data")] public TemplateData TemplateData { get; set; }
    }

    public class TemplateData
    {
        public string CompanyName { get; set; }
    }

    public class SendTo
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
    }

    public class SentFrom
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
    }

    public class Attachment
    {
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("content-type")] public string Type { get; set; }
        [JsonProperty("filename")] public string FileName { get; set; }
    }
}
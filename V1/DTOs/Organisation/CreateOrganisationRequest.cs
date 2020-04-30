using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoviIDApiCore.V1.DTOs.Organisation
{
    public class CreateOrganisationRequest
    {
        [JsonProperty("event_id")] 
        public string Id { get; set; }
        [JsonProperty("event_type")] 
        public string Type { get; set; }
        [JsonProperty("form_response")] 
        public FormResponse FormResponse { get; set; }
    }

    public class Properties
    {
    }

    public class Fields
    {
        [JsonProperty("id")] 
        public string Id { get; set; }
        [JsonProperty("title")] 
        public string Title { get; set; }
        [JsonProperty("type")] 
        public string Type { get; set; }
        [JsonProperty("ref")] 
        public string Reference { get; set; }
        [JsonProperty("properties")] 
        public Properties Properties { get; set; }
    }

    public class Definition
    {
        [JsonProperty("id")] 
        public string Id { get; set; }
        [JsonProperty("title")] 
        public string Title { get; set; }
        [JsonProperty("fields")] 
        public List<Fields> Fields { get; set; }
    }

    public class AnswerFields
    {
        [JsonProperty("id")] 
        public string Id { get; set; }
        [JsonProperty("type")] 
        public string Type { get; set; }
        [JsonProperty("ref")] 
        public string Reference { get; set; }
    }

    public class Choice
    {
        [JsonProperty("label")] 
        public string Label { get; set; }
    }

    public class Answer
    {
        [JsonProperty("type")] 
        public string Type { get; set; }
        [JsonProperty("text")] 
        public string Text { get; set; }
        [JsonProperty("field")] 
        public AnswerFields Field { get; set; }
        [JsonProperty("number")] 
        public int? Number { get; set; }
        [JsonProperty("choice")] 
        public Choice Choice { get; set; }
        [JsonProperty("email")] 
        public string Email { get; set; }
    }

    public class FormResponse
    {
        [JsonProperty("form_id")] 
        public string Id { get; set; }
        [JsonProperty("token")] 
        public string Token { get; set; }
        [JsonProperty("landed_at")] 
        public DateTime LandedAt { get; set; }
        [JsonProperty("submitted_at")] 
        public DateTime SubmittedAt { get; set; }
        [JsonProperty("definition")] 
        public Definition Definition { get; set; }
        [JsonProperty("answers")] 
        public List<Answer> Answers { get; set; }
    }
}
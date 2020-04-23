namespace CoviIDApiCore.Models.AppSettings
{
    public class SendGridCredentials
    {
        public string Key { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
    }
}
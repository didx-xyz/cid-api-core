namespace CoviIDApiCore.V1.DTOs.Authentication
{
    public class RequestResendOtp
    {
        public string SessionId { get; set; }
        public string MobileNumberReference { get; set; }
    }
}
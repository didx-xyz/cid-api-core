namespace CoviIDApiCore.V1.DTOs.Authentication
{
    public class RequestResendOtp
    {
        public string WalletId { get; set; }
        public long MobileNumber { get; set; }
    }
}
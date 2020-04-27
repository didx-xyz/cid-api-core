namespace CoviIDApiCore.V1.DTOs.Authentication
{
    public class RequestResendOtp
    {
        public string WalletId { get; set; }
        public int MobileNumber { get; set; }
    }
}
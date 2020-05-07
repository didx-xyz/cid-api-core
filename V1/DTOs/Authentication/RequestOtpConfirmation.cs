using System.ComponentModel.DataAnnotations;
using CoviIDApiCore.V1.DTOs.Wallet;
using CoviIDApiCore.V1.DTOs.WalletTestResult;

namespace CoviIDApiCore.V1.DTOs.Authentication
{
    public class RequestOtpConfirmation
    {
        [Required]
        public int Otp { get; set; }
        [Required]
        public string SessionId { get; set; }

        public TestResultRequest TestResult { get; set; }
        public WalletDetailsRequest WalletDetails { get; set; }
    }

    public class WalletDetailsRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public IdType IdType { get; set; }
        public string IdValue { get; set; }
    }
}
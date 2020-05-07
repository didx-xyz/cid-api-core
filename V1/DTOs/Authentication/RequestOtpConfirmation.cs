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
        [StringLength(40)]
        public string SessionId { get; set; }

        public TestResultRequest TestResult { get; set; }
        public WalletDetailsRequest WalletDetails { get; set; }
    }

    public class WalletDetailsRequest
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public string Photo { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public IdType IdType { get; set; }
        [StringLength(13)]
        public string IdValue { get; set; }
    }
}
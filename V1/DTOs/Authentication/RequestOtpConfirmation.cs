using System;
using System.ComponentModel.DataAnnotations;
using CoviIDApiCore.V1.DTOs.Wallet;
using CoviIDApiCore.V1.DTOs.WalletTestResult;
using Laboratory = CoviIDApiCore.V1.DTOs.WalletTestResult.Laboratory;

namespace CoviIDApiCore.V1.DTOs.Authentication
{
    public class RequestOtpConfirmation
    {
        [Required]
        public int Otp { get; set; }
        [Required]
        public string SessionId { get; set; }

        public TestResult TestResult { get; set; }
        public WalletDetails WalletDetails { get; set; }
    }

    public class TestResult
    {
        public DateTime TestedAt { get; set; }
        public DateTime IssuedAt { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public Laboratory Laboratory { get; set; }
        public string ReferenceNumber { get; set; }
        public bool HasConsent { get; set; }
    }

    public class WalletDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public IdType IdType { get; set; }
        public string IdValue { get; set; }
    }
}
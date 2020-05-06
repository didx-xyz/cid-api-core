using System.ComponentModel.DataAnnotations;
using CoviIDApiCore.V1.DTOs.Credentials;

namespace CoviIDApiCore.V1.DTOs.Authentication
{
    public class RequestOtpConfirmation
    {
        [Required]
        public int Otp { get; set; }
        [Required]
        public string WalletId { get; set; }
        [Required]
        public string SessionId { get; set; }

        public CovidTestCredentialParameters CovidTest { get; set; }
        public PersonCredentialParameters Person { get; set; }
    }
}
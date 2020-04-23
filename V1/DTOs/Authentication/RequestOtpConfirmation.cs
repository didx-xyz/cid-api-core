using System.ComponentModel.DataAnnotations;
using CoviIDApiCore.V1.Constants;

namespace CoviIDApiCore.V1.DTOs.Authentication
{
    public class RequestOtpConfirmation
    {
        [Required]
        [StringLength(13, MinimumLength = 10, ErrorMessage = Messages.Mobile_NumberInvalid)]
        public string MobileNumber { get; set; }

        [Required]
        public int Otp { get; set; }

        [Required]
        public string WalletId { get; set; }
    }
}
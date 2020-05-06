using CoviIDApiCore.V1.DTOs.Credentials;
using System.ComponentModel.DataAnnotations;

namespace CoviIDApiCore.V1.DTOs.Wallet
{
    public class WalletDetailRequest
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string MobileNumber { get; set; }
        public IdType IdentificationType { get; set; }
        [StringLength(50)]
        public string IdentificationValue { get; set; }
    }
}

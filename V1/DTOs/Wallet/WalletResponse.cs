using System;

namespace CoviIDApiCore.V1.DTOs.Wallet
{
    public class WalletResponse
    {
        public Guid WalletId { get; set; }
        public string MobileNumber { get; set; }
        public string PhotoUrl { get; set; }
    }
}

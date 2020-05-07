using System;

namespace CoviIDApiCore.V1.DTOs.Wallet
{
    public class WalletResponse
    {
        public Guid WalletId { get; set; }
        public Guid SessionId { get; set; }
    }
}

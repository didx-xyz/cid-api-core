using System;
using System.Collections.Generic;

namespace CoviIDApiCore.Models.Database
{
    public class Wallet : BaseModel<Guid>
    {
        public string WalletIdentifier { get; set; }
        public virtual IList<OtpToken> Tokens { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
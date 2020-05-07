using System;
using System.Collections.Generic;

namespace CoviIDApiCore.Models.Database
{
    public class Wallet : BaseModel<Guid>
    {
        public string SessionId { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumberReference { get; set; }
        public DateTime MobileNumberVerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
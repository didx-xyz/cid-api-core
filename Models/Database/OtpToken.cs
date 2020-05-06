using System;
using System.Numerics;

namespace CoviIDApiCore.Models.Database
{
    public class OtpToken : BaseModel<BigInteger>
    {
        public string SessionId { get; set; }
        public int Code { get; set; }
        public string MobileNumber { get; set; }
        public bool isUsed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
﻿using CoviIDApiCore.V1.DTOs.Credentials;
using System;

namespace CoviIDApiCore.Models.Database
{
    public class WalletDetail : BaseModel<Guid>
    {
        public virtual Wallet Wallet { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IdType IdType { get; set; }
        public string IdValue { get; set; }
    }
}

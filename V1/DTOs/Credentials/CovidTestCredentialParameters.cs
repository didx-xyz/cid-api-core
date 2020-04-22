using CoviIDApiCore.V1.DTOs.Wallet;
using System;

namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CovidTestCredentialParameters
    {
        public DateTime TestDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public CovidStatus CovidStatus { get; set; }
        public Labratory Labratory { get; set; }
        public string ReferenceNumber { get; set; }
    }
}

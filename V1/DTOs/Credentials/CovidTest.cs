using System;

namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CovidTest
    {
        public DateTime TestDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public CovidStatus CovidStatus { get; set; }
    }
}

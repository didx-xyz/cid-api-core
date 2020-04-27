using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Wallet;
using System;

namespace CoviIDApiCore.Models.Database
{
    public class CovidTest : BaseModel<Guid>
    {
        public string WalletId { get; set; }
        public DateTime PermissionGrantedAt { get; set; }
        public bool CredentialsVerified { get; set; }


        public DateTime DateTested { get; set; }
        internal DateTime DateIssued { get; set; }
        public CovidStatus CovidStatus { get; set; }
        public Laboratory Laboratory { get; set; }
        public string ReferenceNumber { get; set; }
        public bool HasConsent { get; set; }
        public CovidTest()
        {
        }
    }
}

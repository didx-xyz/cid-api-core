using CoviIDApiCore.V1.DTOs.Wallet;
using System;
using CoviIDApiCore.V1.DTOs.Authentication;

namespace CoviIDApiCore.Models.Database
{
    public class WalletDetail : BaseModel<Guid>
    {
        public virtual Wallet Wallet { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public IdType IdType { get; set; }
        public string IdValue { get; set; }

        public WalletDetail()
        {
        }

        public WalletDetail(WalletDetailsRequest detailsRequest)
        {
            FirstName = detailsRequest.FirstName;
            LastName = detailsRequest.LastName;
            Email = detailsRequest.Email;
            PhotoUrl = detailsRequest.Photo;
            IdType = detailsRequest.IdType;
            IdValue = detailsRequest.IdValue;
        }
    }
}

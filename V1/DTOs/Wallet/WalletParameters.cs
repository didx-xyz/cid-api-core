namespace CoviIDApiCore.V1.DTOs.Wallet
{
    public class WalletParameters
    {
        /// <summary>
        /// An identifier for the wallet
        /// This needs to be unique
        /// </summary>
        public string WalletId { get; set; }
        /// <summary>
        /// The name of the users wallet. This will be shown in the listed connections
        /// </summary>
        public string OwnerName { get; set; }
    }
}

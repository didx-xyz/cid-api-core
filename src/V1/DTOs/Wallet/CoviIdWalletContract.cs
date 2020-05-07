namespace CoviIDApiCore.V1.DTOs.Wallet
{
    public class CoviIdWalletContract
    {
        public string CovidStatusUrl { get; set; }
        /// <summary>
        /// 64 bit string of an image
        /// </summary>
        public string Picture { get; set; }
        public string WalletId { get; set; }
    }
}

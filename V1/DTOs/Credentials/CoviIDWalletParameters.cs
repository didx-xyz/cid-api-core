using CoviIDApiCore.V1.DTOs.Wallet;

namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CoviIdWalletParameters
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TelNumber { get; set; }
        public string Picture { get; set; }
        public CovidTest CovidTest { get; set; }
        public IdentificationTypes IdentificationType { get; set; }
        /// <summary>
        /// Identification Value
        /// </summary>
        public string Identification { get; set; }
        public Labratory Labratory { get; set; }
    }
}

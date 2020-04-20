namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CoviIDWalletParameters
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        /// <summary>
        /// Persons Identification number
        /// </summary>
        public string TelNumber { get; set; }
        /// <summary>
        /// Base 64 string of a photo
        /// </summary>
        public string Picture { get; set; }
        public CovidTest CovidTest { get; set; }
    }
}

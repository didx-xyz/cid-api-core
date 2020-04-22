namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class IdentificationCredentialParameter
    {
        /// <summary>
        /// Type of Identification. SA ID doc or Passport
        /// </summary>
        public IdentificationTypes IdentificationType { get; set; }
        /// <summary>
        /// Identification Value
        /// </summary>
        public string Identification { get; set; }
    }
}

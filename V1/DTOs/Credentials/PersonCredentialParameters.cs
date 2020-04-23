namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class PersonCredentialParameters
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public int MobileNumber { get; set; }
        /// <summary>
        /// Type of Identification. SA ID doc or Passport
        /// </summary>
        public IdentificationTypes IdentificationType { get; set; }
        /// <summary>
        /// Identification Value
        /// </summary>
        public string IdentificationValue { get; set; }
    }
}

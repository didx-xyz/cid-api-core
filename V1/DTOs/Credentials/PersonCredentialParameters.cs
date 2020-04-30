using System.ComponentModel.DataAnnotations;

namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class PersonCredentialParameters
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public string Photo { get; set; }
        public long MobileNumber { get; set; }
        /// <summary>
        /// Type of Identification. SA ID doc or Passport
        /// </summary>
        public IdentificationTypes IdentificationType { get; set; }
        /// <summary>
        /// Identification Value
        /// </summary>
        [StringLength(50)]
        public string IdentificationValue { get; set; }
    }
}

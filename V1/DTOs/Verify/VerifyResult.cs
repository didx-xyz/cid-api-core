namespace CoviIDApiCore.V1.DTOs.Verify
{
    public class VerifyResult
    {
        public string Picture { get; set; }

        /// <summary>
        /// A string interpertation of the enum 
        /// </summary>
        public string CovidStatus { get; set; }
        /// <summary>
        /// The enum value the covid status
        /// </summary>
        public int Status { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}

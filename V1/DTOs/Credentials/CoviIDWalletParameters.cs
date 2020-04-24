namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CoviIdWalletParameters
    {
        public CovidTestCredentialParameters CovidTest { get; set; }
        public PersonCredentialParameters Person { get; set; }
    }
}

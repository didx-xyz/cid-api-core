using CoviIDApiCore.V1.DTOs.Wallet;

namespace CoviIDApiCore.V1.DTOs.Credentials
{
    public class CoviIdWalletParameters
    {
        public CovidTestCredentialParameters CovidTest { get; set; }
        public PersonalDetialsCredentialParameters PersonalDetials { get; set; }
        public IdentificationCredentialParameter Identification { get; set; }
    }
}

using CoviIDApiCore.V1.DTOs.Credentials;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface ICredentialService
    {
        Task<CredentialsContract> CreatePersonalDetials(string connectionId, PersonalDetialsCredentialParameters personalDetials);
        Task<CredentialsContract> CreateCovidTest(string connectionId, CovidTestCredentialParameters covidTestCredential);
        Task<CredentialsContract> CreateIdentification(string connectionId, IdentificationCredentialParameter identification);

    }
}

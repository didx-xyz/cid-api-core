namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IAuthenticationService
    {
        bool IsAuthorized(string apiKey);
    }
}
namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IQRCodeService
    {
        string GenerateQRCode(string id);
    }
}
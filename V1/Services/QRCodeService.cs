using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using QRCoder;

namespace CoviIDApiCore.V1.Services
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IConfiguration _configuration;

        public QRCodeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateQRCode(string id)
        {
            var baseUrl = _configuration.GetValue<string>("CoviIDBaseUrl");

            var qrGenerator = new QRCodeGenerator();

            var qrCodeData = qrGenerator.CreateQrCode($"{baseUrl}{UrlConstants.PartialRoutes[UrlConstants.Routes.Organisation]}/{id}", QRCodeGenerator.ECCLevel.Q);

            var qrCode = new Base64QRCode(qrCodeData);

            return $"{qrCode.GetGraphic(20)}";
        }
    }
}
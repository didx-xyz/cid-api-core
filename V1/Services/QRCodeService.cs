using System;
using System.IO;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.Interfaces.Services;
using IronBarCode;
using Microsoft.Extensions.Configuration;

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

            var qrCode = QRCodeWriter.CreateQrCode($"{baseUrl}{UrlConstants.PartialRoutes[UrlConstants.Routes.Organisation]}/{id}", 400, QRCodeWriter.QrErrorCorrectionLevel.Medium);

            if(!qrCode.Verify())
                throw new QRException(Messages.QR_Failed);

            var binaryData = qrCode.ToPngBinaryData();

            return $"{Convert.ToBase64String(binaryData)}";
        }
    }
}
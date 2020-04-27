using System;
using System.IO;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.Interfaces.Services;
using IronBarCode;

namespace CoviIDApiCore.V1.Services
{
    public class QRCodeService : IQRCodeService
    {
        public string GenerateQRCode(string id)
        {
            var qrCode = QRCodeWriter.CreateQrCode(id, 400, QRCodeWriter.QrErrorCorrectionLevel.Medium);

            if(!qrCode.Verify())
                throw new QRException(Messages.QR_Failed);

            var binaryData = qrCode.ToPngBinaryData();

            return $"{Convert.ToBase64String(binaryData)}";
        }
    }
}
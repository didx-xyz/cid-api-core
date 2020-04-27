using System.Threading.Tasks;

using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.System;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface IEmailService
    {
        Task<Response> SendEmail(string receiverEmail, string receiverName, string qrCode, DefinitionConstants.EmailTemplates template);
    }
}
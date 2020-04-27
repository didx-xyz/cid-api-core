using System.Net;
using System.Threading.Tasks;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.SendGrid;
using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridBroker _sendGridBroker;
        private static IConfiguration _configuration;

        private const string _png = "image/png";

        public EmailService(ISendGridBroker sendGridBroker, IConfiguration configuration)
        {
            _sendGridBroker = sendGridBroker;
            _configuration = configuration;
        }

        public async Task<Response> SendEmail(string receiverEmail, string receiverName, string qrCode, DefinitionConstants.EmailTemplates template)
        {
            var message = ConstructMessage(receiverEmail, receiverName, qrCode, template);

            await _sendGridBroker.SendEmail(message);

            return new Response(true, HttpStatusCode.OK);
        }

        private static SendGridTemplate ConstructMessage(string email, string name, string qrCode, DefinitionConstants.EmailTemplates template)
        {
            SendTo[] recipient =
            {
                new SendTo()
                {
                    Name = name,
                    Email = email
                }
            };

            Personalizations[] personalizations =
            {
                new Personalizations()
                {
                    To = recipient,
                    Subject = DefinitionConstants.EmailSubjects[template],
                    TemplateData = new TemplateData()
                    {
                        CompanyName = name
                    }
                }
            };

            Attachment[] attachments =
            {
                new Attachment()
                {
                    Content = qrCode,
                    FileName = $"{name}.png",
                    Type = _png
                }
            };

            var from = new SentFrom()
            {
                Name = _configuration.GetValue<string>("SendgridCredentials:From"),
                Email = _configuration.GetValue<string>("SendgridCredentials:FromAddress")
            };

            return new SendGridTemplate()
            {
                Personalizations = personalizations,
                TemplateId = DefinitionConstants.TemplateIds[template],
                Attachments = attachments,
                From = from
            };
        }
    }
}
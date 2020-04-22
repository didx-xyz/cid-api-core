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

        public EmailService(ISendGridBroker sendGridBroker, IConfiguration configuration)
        {
            _sendGridBroker = sendGridBroker;
            _configuration = configuration;
        }

        public async Task<Response> SendEmail(string receiverEmail, string receiverName, string qrCode, ParameterConstants.EmailTemplates template)
        {
            var message = ConstructMessage(receiverEmail, receiverName, qrCode, template);

            await _sendGridBroker.SendEmail(message);

            return new Response(true, HttpStatusCode.OK);
        }

        private static SendGridTemplate ConstructMessage(string email, string name, string qrCode, ParameterConstants.EmailTemplates template)
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
                    Subject = ParameterConstants.EmailSubjects[template],
                    TemplateData = new TemplateData()
                    {
                        CompanyName = name,
                        QR = qrCode
                    }
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
                TemplateId = ParameterConstants.TemplateIds[template],
                From = from
            };
        }
    }
}
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.SendGrid;
using CoviIDApiCore.V1.Interfaces.Brokers;

namespace CoviIDApiCore.V1.Brokers
{
    public class SendGridBroker : ISendGridBroker
    {
        private readonly HttpClient _httpClient;
        private static readonly string _partialRoot = UrlConstants.PartialRoutes[UrlConstants.Routes.Sendgrid];
        private const string _applicationJson = "application/json";

        public SendGridBroker(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmail(object payload)
        {
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue(_applicationJson));

            var response = await _httpClient.PostAsJsonAsync(_partialRoot, payload);

            if(!response.IsSuccessStatusCode)
                throw new SendGridException(await response.Content.ReadAsStringAsync());
        }
    }
}
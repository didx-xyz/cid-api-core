using System.Net.Http;
using System.Threading.Tasks;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.Interfaces.Brokers;

namespace CoviIDApiCore.V1.Brokers
{
    public class SendGridBroker : ISendGridBroker
    {
        private readonly HttpClient _httpClient;
        private static readonly string _partialRoot = UrlConstants.PartialRoutes[UrlConstants.Routes.Sendgrid];

        public SendGridBroker(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmail(object payload)
        {
            var response = await _httpClient.PostAsJsonAsync(_partialRoot, payload);

            if (!response.IsSuccessStatusCode)
                throw new SendGridException(await response.Content.ReadAsStringAsync());
        }
    }
}
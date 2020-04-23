using System.Net.Http;
using System.Threading.Tasks;

using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.Interfaces.Brokers;

namespace CoviIDApiCore.V1.Brokers
{
    public class ClickatellBroker : IClickatellBroker
    {
        private readonly HttpClient _httpClient;
        private static readonly string _sendPartialRoot = UrlConstants.PartialRoutes[UrlConstants.Routes.ClickatellSend];
        private static readonly string _statusPartialRoot = UrlConstants.PartialRoutes[UrlConstants.Routes.ClickatellStatus];

        public ClickatellBroker(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendSms(object payload)
        {
            var response = await _httpClient.PostAsJsonAsync(_sendPartialRoot, payload);

            if(!response.IsSuccessStatusCode)
                throw new ClickatellException(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> GetStatus(string messageId)
        {
            var response = await _httpClient.GetAsync(string.Format(_statusPartialRoot, messageId));

            if(!response.IsSuccessStatusCode)
                throw new ClickatellException(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadAsStringAsync();
        }
    }
}
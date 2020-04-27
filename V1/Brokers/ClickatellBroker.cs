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
    }
}
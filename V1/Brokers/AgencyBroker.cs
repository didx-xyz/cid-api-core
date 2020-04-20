using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.Interfaces.Brokers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoviIDApiCore.V1.Constants;

namespace CoviIDApiCore.V1.Brokers
{
    public class AgencyBroker : IAgencyBroker
    {
        private readonly HttpClient _httpClient;

        private static readonly string partialRoot = UrlConstants.PartialRoutes[UrlConstants.Routes.Agency];
        private static readonly string applicationJson = "application/json";

        public AgencyBroker(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Common  
        public async Task<string> UploadFiles(string file, string fileName)
        {
            var base64Array = Convert.FromBase64String(file);
            var filePath = Path.Combine($"{Environment.CurrentDirectory}/{fileName}.png");
            File.WriteAllBytes(filePath, base64Array);

            var multipartContent = new MultipartFormDataContent
            {
                {
                    new ByteArrayContent(File.ReadAllBytes(filePath)),
                    "uploadedFiles", Path.GetFileName(filePath)
                },
                {new StringContent(fileName), "filename"},
                {new StringContent("image/png"), "contentType"}
            };

            File.Delete(filePath);

            var response = await _httpClient.PostAsync($"{partialRoot}/common/upload", multipartContent);
            response = await ValidateResponse(response);
            return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
        }

        #endregion

        #region Connections
        public async Task<ConnectionContract> CreateInvitation(ConnectionParameters connectionParameters, string agentId)
        {
            var content = new StringContent(JsonConvert.SerializeObject(connectionParameters), Encoding.UTF8, applicationJson);
            
            var response = await _httpClient.PostAsync($"{partialRoot}connections", content);
            response = await ValidateResponse(response);

            return JsonConvert.DeserializeObject<ConnectionContract>(await response.Content.ReadAsStringAsync());
        }
        #endregion

        #region Credentials
        public async Task<CredentialsContract> GetCredential(string credentialId, string agentId)
        {
            var response = await _httpClient.GetAsync($"{partialRoot}/credentials?{credentialId}");
            response = await ValidateResponse(response);
            return JsonConvert.DeserializeObject<CredentialsContract>(await response.Content.ReadAsStringAsync());
        }

        public async Task<CredentialsContract> SendCredentials(CredentialOfferParameters credentials, string agentId)
        {
            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, applicationJson);

            var response = await _httpClient.PostAsync($"{partialRoot}/credentials", content);
            response = await ValidateResponse(response);

            return JsonConvert.DeserializeObject<CredentialsContract>(await response.Content.ReadAsStringAsync());
        }
        #endregion

        #region Verifications
       
        #endregion



        private async Task<HttpResponseMessage> ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return response;
            
            //TODO: log the broker response
            var message = await response.Content.ReadAsStringAsync();
            throw new StreetCredBrokerException($"{message} Broker status code: {response.StatusCode}");
        }
    }
}

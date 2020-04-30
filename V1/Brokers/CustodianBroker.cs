using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.Connection;
using CoviIDApiCore.V1.DTOs.Credentials;
using CoviIDApiCore.V1.DTOs.Verifications;
using CoviIDApiCore.V1.DTOs.Wallet;
using CoviIDApiCore.V1.Interfaces.Brokers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Brokers
{
    public class CustodianBroker : ICustodianBroker
    {
        private readonly HttpClient _httpClient;

        private static readonly string _partialRoot = UrlConstants.PartialRoutes[UrlConstants.Routes.Custodian];
        private static readonly string _applicationJson = "application/json";

        public CustodianBroker(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Wallet
        public async Task<List<WalletContract>> GetWallets()
        {
            var response = await _httpClient.GetAsync($"{_partialRoot}wallets");
            response = await ValidateResponse(response);
            return JsonConvert.DeserializeObject<List<WalletContract>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<WalletContract> CreateWallet(WalletParameters walletParameters)
        {
            var content = new StringContent(JsonConvert.SerializeObject(walletParameters), Encoding.UTF8, _applicationJson);

            var response = await _httpClient.PostAsync($"{_partialRoot}wallets", content);
            response = await ValidateResponse(response);
            return JsonConvert.DeserializeObject<WalletContract>(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteWallet(string walletId)
        {
            var response = await _httpClient.DeleteAsync($"{_partialRoot}wallets?walletId={walletId}");
            await ValidateResponse(response);
        }
        #endregion

        #region Connections
        public async Task<ConnectionContract> AcceptInvitation(string invitation, string walletId)
        {
            var formVariables = new List<KeyValuePair<string, string>>();

            formVariables.Add(new KeyValuePair<string, string>("invitation", invitation));

            var formContent = new FormUrlEncodedContent(formVariables);

            var response = await _httpClient.PostAsync($"{_partialRoot}{walletId}/connections/invitation", formContent);
            response = await ValidateResponse(response);
            return JsonConvert.DeserializeObject<ConnectionContract>(await response.Content.ReadAsStringAsync());
        }
        #endregion

        #region Credentials
        public async Task AcceptCredential(string walletId, string credentialId)
        {
            var content = new StringContent(string.Empty, Encoding.UTF8, _applicationJson);

            var response = await _httpClient.PostAsync($"{_partialRoot}{walletId}/credentials/{credentialId}", content);
            response = await ValidateResponse(response);
        }

        public async Task<List<CredentialsContract>> GetCredentialsByConnectionId(string walletId, string connectionId)
        {
            var response = await _httpClient.GetAsync($"{_partialRoot}{walletId}/credentials/connections/{connectionId}");
            response = await ValidateResponse(response);
            return JsonConvert.DeserializeObject<List<CredentialsContract>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<CredentialsContract>> GetCredentials(string walletId)
        {
            var response = await _httpClient.GetAsync($"{_partialRoot}{walletId}/credentials");
            response = await ValidateResponse(response);

            return JsonConvert.DeserializeObject<List<CredentialsContract>>(await response.Content.ReadAsStringAsync());
        }
        #endregion

        #region Verifications
        public async Task AcceptVerification(string walletId, string verificationId)
        {
            var response = await _httpClient.GetAsync($"{_partialRoot}{walletId}/verifications/{verificationId}/autoSelect");

            response = await ValidateResponse(response);
            return;
        }

        public async Task<List<VerificationContract>> GetVerifications(string walletId, string connectionId)
        {
            var response = await _httpClient.GetAsync($"{_partialRoot}{walletId}/verifications/connection/{connectionId}");
            response = await ValidateResponse(response);

            return JsonConvert.DeserializeObject<List<VerificationContract>>(await response.Content.ReadAsStringAsync());
        }



        #endregion

        private async Task<HttpResponseMessage> ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return response;

            var message = await response.Content.ReadAsStringAsync();
            throw new StreetCredBrokerException($"{message} Broker status code: {response.StatusCode}");
        }
    }
}

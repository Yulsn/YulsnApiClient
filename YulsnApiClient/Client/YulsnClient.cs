using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        private readonly HttpClient httpClient;

        public YulsnClient(HttpClient httpClient, IConfiguration config)
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.SetYulsnApiHost("api.loyaltii.com");
            this.httpClient = httpClient;

            if (config != null)
            {
                if (config["yulsn-api-key"] != null)
                    httpClient.SetYulsnApiKey(config["yulsn-api-key"]);

                if (config["yulsn-api-host"] != null)
                    httpClient.SetYulsnApiHost(config["yulsn-api-host"]);
            }
        }

        public void SetYulsnApiKey(string apiKey) => httpClient.SetYulsnApiKey(apiKey);
        public void SetYulsnApiHost(string apiHost) => httpClient.SetYulsnApiHost(apiHost);

        private Task<T> sendAsync<T>(string url) => sendAsync<T>(new HttpRequestMessage(HttpMethod.Get, url));
        private Task<T> sendAsync<T>(HttpMethod method, string url) => sendAsync<T>(new HttpRequestMessage(method, url));
        private async Task<T> sendAsync<T>(HttpRequestMessage request)
        {
            using (var response = await httpClient.SendAsync(request))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return default(T);
                }

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
        }

        private HttpContent json<T>(T model) => new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
    }
}
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        private readonly HttpClient httpClient;

        public YulsnClient(HttpClient httpClient, IConfiguration config)
        {
            string apiKey = config["yulsn-api-key"];
            string apiHost = config["yulsn-api-host"];
            this.httpClient = setupClient(httpClient, apiKey, apiHost);
        }

        public YulsnClient(HttpClient httpClient, string apiKey, string apiHost)
        {
            this.httpClient = setupClient(httpClient, apiKey, apiHost);
        }

        HttpClient setupClient(HttpClient httpClient, string apiKey, string apiHost)
        {
            httpClient.BaseAddress = new Uri($"https://{apiHost}/");

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-ApiKey", apiKey);

            return httpClient;
        }
    }
}
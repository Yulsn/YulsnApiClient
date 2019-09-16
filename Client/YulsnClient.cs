using System;
using System.Net.Http;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        private readonly HttpClient httpClient;

        public YulsnClient(HttpClient httpClient)
        {
            string apiKey = "";

            httpClient.BaseAddress = new Uri("https://api.loyaltii.com/");

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-ApiKey", apiKey);

            this.httpClient = httpClient;
        }        
    }
}
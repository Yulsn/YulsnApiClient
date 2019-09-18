using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using YulsnApiClient.Helpers;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        private readonly HttpClient httpClient;

        public YulsnClient(HttpClient httpClient, IConfiguration config)
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            this.httpClient = httpClient;

            if (config["yulsn-api-key"] != null)
                httpClient.SetYulsnApiKey(config["yulsn-api-key"]);

            if (config["yulsn-api-host"] != null)
                httpClient.SetYulsnApiHost(config["yulsn-api-host"]);            
        }        
    }
}
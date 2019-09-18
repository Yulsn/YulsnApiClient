using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace YulsnApiClient.Client
{
    public static class HttpClientHelper
    {
        public static void SetYulsnApiHost(this HttpClient httpClient, string apiHost) => httpClient.BaseAddress = new Uri($"https://{apiHost}/");
        public static void SetYulsnApiKey(this HttpClient httpClient, string apiKey) => httpClient.DefaultRequestHeaders.Add("X-ApiKey", apiKey);
    }
}

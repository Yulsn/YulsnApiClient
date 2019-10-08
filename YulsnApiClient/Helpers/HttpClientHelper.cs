using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace YulsnApiClient.Client
{
    public static class HttpClientHelper
    {
        const string keyHeaderName = "X-ApiKey";
        public static void SetYulsnApiHost(this HttpClient httpClient, string apiHost) => httpClient.BaseAddress = new Uri($"https://{apiHost}/");
        public static void SetYulsnApiKey(this HttpClient httpClient, string apiKey)
        {
            httpClient.DefaultRequestHeaders.Remove(keyHeaderName);
            httpClient.DefaultRequestHeaders.Add(keyHeaderName, apiKey);
        }
    }
}

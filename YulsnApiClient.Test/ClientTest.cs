using System;
using System.Linq;
using System.Net.Http;
using Xunit;
using YulsnApiClient.Client;

namespace YulsnApiClient.Test
{
    public class ClientTest
    {
        const string apiHost = "api.loyaltii.test";
        const string apiKey = "xxxx";
        readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public void SetYulsnApiHost()
        {
            httpClient.SetYulsnApiHost(apiHost);
            Assert.Equal(new Uri($"https://{apiHost}/"), httpClient.BaseAddress);
        }

        [Fact]
        public void SetYulsnApiKey()
        {
            httpClient.SetYulsnApiKey(apiKey);
            Assert.Equal(apiKey, httpClient.DefaultRequestHeaders.GetValues("X-ApiKey").FirstOrDefault());
        }

        [Fact]
        public void DefaultConfig()
        {
            new YulsnClient(httpClient, null);
            Assert.Equal(new Uri($"https://api.loyaltii.com/"), httpClient.BaseAddress);

            Assert.Equal("application/json", httpClient.DefaultRequestHeaders.Accept.ToString());
        }
    }
}

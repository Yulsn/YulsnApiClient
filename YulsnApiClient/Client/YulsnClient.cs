using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    public enum YulsnApiVersion
    {
        V1, V2
    }

    public partial class YulsnClient
    {
        public const string HttpClientName = "YulsnHttpClient";

        public int AccountId { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;
        private string apiKey = null;
        private string apiHost = null;
        private string apiV1baseUrl = null;
        private string apiV2baseUrl = null;

        public YulsnClient(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            apiHost = "api.yulsn.io";

            if (config != null)
            {
                if (config["yulsn-api-key"] != null)
                    apiKey = config["yulsn-api-key"];

                if (config["yulsn-api-host"] != null)
                    apiHost = config["yulsn-api-host"];

                if (config["yulsn-api-accountid"] != null && int.TryParse(config["yulsn-api-accountid"], out int accountId))
                    AccountId = accountId;

                if (config["yulsn-api-v1-baseurl"] != null)
                    apiV1baseUrl = config["yulsn-api-v1-baseurl"];

                if (config["yulsn-api-v2-baseurl"] != null)
                    apiV2baseUrl = config["yulsn-api-v2-baseurl"];
            }
        }

        HttpClient GetClient(YulsnApiVersion apiVersion)
        {
            HttpClient client = _httpClientFactory.CreateClient(HttpClientName);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("X-ApiKey", apiKey);
            client.BaseAddress = new Uri("https://" + apiHost, UriKind.Absolute);

            switch (apiVersion)
            {
                case YulsnApiVersion.V1:
                    if (apiV1baseUrl != null)
                        client.BaseAddress = new Uri(apiV1baseUrl, UriKind.Absolute);
                    break;
                case YulsnApiVersion.V2:
                    if (apiV2baseUrl != null)
                        client.BaseAddress = new Uri(apiV2baseUrl, UriKind.Absolute);
                    break;
                default:
                    break;
            }
            return client;
        }

        public void SetYulsnApiKey(string apiKey) =>
            this.apiKey = apiKey;

        public void SetYulsnApiHost(string apiHost) =>
            this.apiHost = apiHost;

        public Task<T> SendAsync<T>(string url, YulsnApiVersion apiVersion = YulsnApiVersion.V1) =>
            SendAsync<T>(new HttpRequestMessage(HttpMethod.Get, url), apiVersion);

        public Task<T> SendAsync<T>(HttpMethod method, string url, YulsnApiVersion apiVersion = YulsnApiVersion.V1) =>
            SendAsync<T>(new HttpRequestMessage(method, url), apiVersion);

        public Task<T> SendAsync<T>(HttpMethod method, string url, object body, YulsnApiVersion apiVersion = YulsnApiVersion.V1) =>
            SendAsync<T>(new HttpRequestMessage(method, url) { Content = JsonContent(body) }, apiVersion);

        public async Task<T> SendAsync<T>(HttpRequestMessage request, YulsnApiVersion apiVersion = YulsnApiVersion.V1)
        {
            using (var response = await GetClient(apiVersion).SendAsync(request).ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return default;
                }

                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    if (apiVersion == YulsnApiVersion.V2)
                    {
                        ProblemDetails pd;
                        try
                        {
                            pd = JsonConvert.DeserializeObject<ProblemDetails>(content);
                        }
                        catch
                        {
                            pd = new ProblemDetails { Status = (int)response.StatusCode, Title = response.ReasonPhrase, Detail = content };
                        }

                        throw pd;
                    }
                    else // v1 exception
                    {
                        throw new YulsnRequestException(response.StatusCode, response.ReasonPhrase, content);
                    }
                }

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)content;
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public HttpContent JsonContent<T>(T model) =>
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
    }
}
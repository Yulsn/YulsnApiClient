﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        private readonly HttpClient httpClient;
        static readonly ActivitySource activitySource = new ActivitySource("yulsn.api.client");

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

        public Task<T> SendAsync<T>(string url, string activityName = null) => SendAsync<T>(new HttpRequestMessage(HttpMethod.Get, url), activityName);

        public Task<T> SendAsync<T>(HttpMethod method, string url, string activityName = null) => SendAsync<T>(new HttpRequestMessage(method, url), activityName);

        public Task<T> SendAsync<T>(HttpMethod method, string url, object body, string activityName = null) => SendAsync<T>(new HttpRequestMessage(method, url) { Content = JsonContent(body) }, activityName);

        public async Task<T> SendAsync<T>(HttpRequestMessage request, string activityName = null)
        {
            using (var activity = GetActivity(activityName))
            {
                activity?.SetTag("http.request.method", request.Method.ToString());

                using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    activity?.SetTag("http.response.status_code", (int)response.StatusCode);

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return default;
                    }

                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new YulsnRequestException(response.StatusCode, response.ReasonPhrase, json);
                    }

                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
        }

        public HttpContent JsonContent<T>(T model) => new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        private Activity GetActivity(string activityName)
        {
            if (string.IsNullOrWhiteSpace(activityName))
                return null;

            if (activityName.EndsWith("Async"))
                activityName = activityName.Remove(activityName.Length - 5);

            return activitySource.StartActivity($"Yulsn.ApiClient.{activityName}", ActivityKind.Client);
        }
    }
}
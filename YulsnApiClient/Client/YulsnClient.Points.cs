using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Net.Http;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<YulsnReadPointDto> GetPoint(int pointId) => 
            sendAsync<YulsnReadPointDto>($"/api/v1/Points/{pointId}");

        public Task<List<YulsnReadPointSumDto>> GetPointSums(int contactId, string type = null) =>
            sendAsync<List<YulsnReadPointSumDto>>($"api/v1/Points/Sums?contactId={contactId + (type != null ? $"&type={type}" : "")}");

        public Task CancelPoint(int pointId) =>
            sendAsync<object>(HttpMethod.Post, $"api/v1/Points/{pointId}/Cancel");

        public Task<YulsnReadPointDto> CreatePoint<T>(T yulsnCreatePointDto) where T : YulsnCreatePointDto
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/Points");

            request.Content = json(yulsnCreatePointDto);

            return sendAsync<YulsnReadPointDto>(request);
        }
    }
}
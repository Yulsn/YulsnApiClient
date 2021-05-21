using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<YulsnReadPointDto> GetPointAsync(int pointId) =>
            SendAsync<YulsnReadPointDto>($"/api/v1/Points/{pointId}");

        public Task<List<YulsnReadPointDto>> GetPointsAsync(int contactId, string type = null, DateTimeOffset? dateTimeFrom = null, DateTimeOffset? dateTimeTo = null) =>
            SendAsync<List<YulsnReadPointDto>>($"api/v1/Points?contactId={contactId + (type != null ? $"&type={type}" : "") + (dateTimeFrom != null ? $"&datetimeFrom={HttpUtility.UrlEncode(((DateTimeOffset)dateTimeFrom).ToString("O"))}" : "") + (dateTimeTo != null ? $"&dateTimeTo={HttpUtility.UrlEncode(((DateTimeOffset)dateTimeTo).ToString("O"))}" : "")}");

        public Task<List<YulsnReadPointSumDto>> GetPointSumsAsync(int contactId, string type = null) =>
            SendAsync<List<YulsnReadPointSumDto>>($"api/v1/Points/Sums?contactId={contactId + (type != null ? $"&type={type}" : "")}");

        public Task CancelPointAsync(int pointId) =>
            SendAsync<object>(HttpMethod.Post, $"api/v1/Points/{pointId}/Cancel");

        public Task<YulsnReadPointDto> CreatePointAsync<T>(T yulsnCreatePointDto) where T : YulsnCreatePointDto =>
            SendAsync<YulsnReadPointDto>(HttpMethod.Post, "api/v1/Points", yulsnCreatePointDto);
    }
}
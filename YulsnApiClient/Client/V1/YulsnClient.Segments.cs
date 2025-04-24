using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<int>> GetContactIdsAsync(int segmentId) =>
            SendAsync<List<int>>($"api/v1/Segments/{segmentId}/GetContactIds");

        public Task<List<YulsnContactBaseInfo>> GetContactsBaseInfoAsync(int segmentId) =>
            SendAsync<List<YulsnContactBaseInfo>>($"api/v1/Segments/{segmentId}/GetContactsBaseInfo");

        public Task<YulsnReadSegmentDto> CreateTemplateSegmentAsync(YulsnCreateSegmentTemplate yulsnPostSegmentTemplateDto) =>
            SendAsync<YulsnReadSegmentDto>(HttpMethod.Post, "api/v1/Segments/Template", yulsnPostSegmentTemplateDto);

        public Task<int> GetContactCountAsync(params int[] segmentIds) =>
            SendAsync<int>(HttpMethod.Post, "api/v1/Segments/GetContactCount", new { SegmentIds = segmentIds });
    }
}
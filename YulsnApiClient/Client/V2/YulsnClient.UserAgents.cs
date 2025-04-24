using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<YulsnUserAgent>> GetUserAgentsAsync(List<int> ids) =>
             SendAsync<List<YulsnUserAgent>>(HttpMethod.Post, $"api/v2/{AccountId}/UserAgents", ids, YulsnApiVersion.V2);
    }
}

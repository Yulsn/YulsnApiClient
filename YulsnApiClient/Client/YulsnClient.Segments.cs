using System.Collections.Generic;
using System.Threading.Tasks;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<int>> GetContactIdsAsync(int segmentId) =>
            SendAsync<List<int>>($"/api/v1/Segments/{segmentId}/GetContactIds");
    }
}
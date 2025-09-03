using System.Net.Http;
using System.Threading.Tasks;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task FeedSyncNowAsync(int feedSyncId) =>
            SendAsync<object>(HttpMethod.Post, $"api/v1/FeedSyncs/{feedSyncId}/SyncNow");
    }
}

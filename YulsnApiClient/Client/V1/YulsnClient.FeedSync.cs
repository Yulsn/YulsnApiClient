using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task FeedSyncNowAsync(int feedSyncId) =>
            SendAsync<object>(HttpMethod.Post, $"api/v1/FeedSyncs/{feedSyncId}/SyncNow");
    }
}

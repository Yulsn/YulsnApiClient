using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>
        /// Will merge data from source contact to target contact.
        /// </summary>
        /// <param name="sourceId">Id of Source contact</param>
        /// <param name="targetId">Id of target contact</param>
        /// <returns></returns>
        public Task<YulsnContactMergeResult> MergeContactsAsync(int sourceId, int targetId, YulsnContactMergeRequest body) =>
            SendAsync<YulsnContactMergeResult>(HttpMethod.Post, $"api/v2/{AccountId}/Contacts/{sourceId}/Merge/{targetId}", body, YulsnApiVersion.V2);
    }
}
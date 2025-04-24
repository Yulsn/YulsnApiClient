using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>
        /// Sends a push message to a single contact
        /// </summary>
        public Task<YulsnSendMessageResponse> SendSinglePushMessageAsync(YulsnSendMessageToContactRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Push/Send/Contact", body, YulsnApiVersion.V2);

        /// <summary>
        /// Sends a push message to a bulk of contacts defined by segment(s)
        /// </summary>
        public Task<YulsnSendMessageResponse> SendBulkPushMessageAsync(YulsnSendMessageToSegmentRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Push/Send/Segment", body, YulsnApiVersion.V2);
    }
}
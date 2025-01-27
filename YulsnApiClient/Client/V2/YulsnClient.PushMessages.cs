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
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Push/Send/Contact", body, nameof(SendSinglePushMessageAsync), YulsnApiVersion.V2);

        /// <summary>
        /// Sends a push message to a bulk of contacts defined by segment(s)
        /// </summary>
        public Task<YulsnSendMessageResponse> SendBulkPushMessageAsync(YulsnSendMessageToSegmentRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Push/Send/Segment", body, nameof(SendBulkPushMessageAsync), YulsnApiVersion.V2);

        /// <summary>
        /// Adds a new push token to a contact
        /// </summary>
        public Task<YulsnPushTokenPostResponse> AddPushTokenToContactAsync(YulsnPushTokenPostRequest body) =>
            SendAsync<YulsnPushTokenPostResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Push/Tokens", body, nameof(AddPushTokenToContactAsync), YulsnApiVersion.V2);

        /// <summary>
        /// Deletes an existing push token from a contact
        /// </summary>
        public Task DeletePushTokenFromContactAsync(YulsnPushTokenDeleteRequest body) =>
            SendAsync<object>(HttpMethod.Delete, $"api/v2/{AccountId}/Messages/Push/Tokens", body, nameof(DeletePushTokenFromContactAsync), YulsnApiVersion.V2);
    }
}
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
        /// Sends a sms message to a single contact
        /// </summary>
        public Task<YulsnSendMessageResponse> SendSingleSmsMessageAsync(YulsnSendSingleMessageRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Sms/SendSingle", body, nameof(SendSingleSmsMessageAsync), YulsnApiVersion.V2);

        /// <summary>
        /// Sends a sms message to a bulk of contacts defined by segment(s)
        /// </summary>
        public Task<YulsnSendMessageResponse> SendBulkSmsMessageAsync(YulsnSendBulkMessageRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Sms/SendBulk", body, nameof(SendBulkSmsMessageAsync), YulsnApiVersion.V2);
    }
}
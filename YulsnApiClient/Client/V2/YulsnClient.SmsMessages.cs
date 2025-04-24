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
        public Task<YulsnSendMessageResponse> SendSingleSmsMessageAsync(YulsnSendMessageToContactRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Sms/Send/Contact", body, YulsnApiVersion.V2);

        /// <summary>
        /// Sends a sms message to a phone number
        /// </summary>
        public Task<YulsnSendMessageResponse> SendSingleSmsMessageAsync(YulsnSendMessageToPhoneRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Sms/Send/Phone", body, YulsnApiVersion.V2);

        /// <summary>
        /// Sends a sms message to a bulk of contacts defined by segment(s)
        /// </summary>
        public Task<YulsnSendMessageResponse> SendBulkSmsMessageAsync(YulsnSendMessageToSegmentRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Sms/Send/Segment", body, YulsnApiVersion.V2);
    }
}
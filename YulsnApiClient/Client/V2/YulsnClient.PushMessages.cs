﻿using System.Collections.Generic;
using System.Net;
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
        public Task<YulsnSendMessageResponse> SendSinglePushMessageAsync(YulsnSendSinglePushMessageRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Push/SendSingle", body, nameof(SendSinglePushMessageAsync));

        /// <summary>
        /// Sends a push message to a bulk of contacts defined by segment(s)
        /// </summary>
        public Task<YulsnSendMessageResponse> SendBulkPushMessageAsync(YulsnSendBulkPushMessageRequest body) =>
            SendAsync<YulsnSendMessageResponse>(HttpMethod.Post, $"api/v2/{AccountId}/Messages/Push/SendBulk", body, nameof(SendBulkPushMessageAsync));
    }
}
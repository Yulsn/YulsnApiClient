using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<YulsnReadEmailBuilderCampaign> CreateEmailBuilderCampaign(YulsnCreateEmailBuilderCampaign yulsnCreateEmailBuilderCampaign)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/EmailBuilderCampaigns");

            request.Content = JsonContent(yulsnCreateEmailBuilderCampaign);

            return SendAsync<YulsnReadEmailBuilderCampaign>(request);
        }

        public Task<YulsnReadEmailBuilderCampaignBlock> CreateEmailBuilderCampaignBlock(int emailBuilderCampaignId, YulsnCreateEmailBuilderCampaignBlock yulsnCreateEmailBuilderCampaignBlock)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/EmailBuilderCampaigns/{emailBuilderCampaignId}/AddBlock");

            request.Content = JsonContent(yulsnCreateEmailBuilderCampaignBlock);

            return SendAsync<YulsnReadEmailBuilderCampaignBlock>(request);
        }

        public Task ScheduleEmailBuilderCampaign(int emailBuilderCampaignId, YulsnCreateScheduleEmailBuilderCampaign yulsnCreateScheduleEmailBuilder)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/EmailBuilderCampaigns/{emailBuilderCampaignId}/Schedule");

            request.Content = JsonContent(yulsnCreateScheduleEmailBuilder);

            return SendAsync<object>(request);
        }

        public Task CancelEmailBuilderCampaign(int emailBuilderCampaignId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/EmailBuilderCampaigns/{emailBuilderCampaignId}/Cancel");
            return SendAsync<object>(request);
        }
    }
}

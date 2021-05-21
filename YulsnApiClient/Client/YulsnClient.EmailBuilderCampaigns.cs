using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<YulsnReadEmailBuilderCampaign> CreateEmailBuilderCampaignAsync(YulsnCreateEmailBuilderCampaign yulsnCreateEmailBuilderCampaign) =>
            SendAsync<YulsnReadEmailBuilderCampaign>(new HttpRequestMessage(HttpMethod.Post, "api/v1/EmailBuilderCampaigns")
            {
                Content = JsonContent(yulsnCreateEmailBuilderCampaign)
            });

        public Task<YulsnReadEmailBuilderCampaignBlock> CreateEmailBuilderCampaignBlockAsync(int emailBuilderCampaignId, YulsnCreateEmailBuilderCampaignBlock yulsnCreateEmailBuilderCampaignBlock) =>
            SendAsync<YulsnReadEmailBuilderCampaignBlock>(new HttpRequestMessage(HttpMethod.Post, $"api/v1/EmailBuilderCampaigns/{emailBuilderCampaignId}/AddBlock")
            {
                Content = JsonContent(yulsnCreateEmailBuilderCampaignBlock)
            });

        public Task ScheduleEmailBuilderCampaignAsync(int emailBuilderCampaignId, YulsnCreateScheduleEmailBuilderCampaign yulsnCreateScheduleEmailBuilder) =>
            SendAsync<object>(new HttpRequestMessage(HttpMethod.Post, $"api/v1/EmailBuilderCampaigns/{emailBuilderCampaignId}/Schedule")
            {
                Content = JsonContent(yulsnCreateScheduleEmailBuilder)
            });

        public Task CancelEmailBuilderCampaignAsync(int emailBuilderCampaignId) =>
            SendAsync<object>(new HttpRequestMessage(HttpMethod.Post, $"api/v1/EmailBuilderCampaigns/{emailBuilderCampaignId}/Cancel"));
    }
}

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<YulsnTriggerMessageResponse>> GetActiveTriggersAsync(YulsnMessageForm form) =>
            SendAsync<List<YulsnTriggerMessageResponse>>(HttpMethod.Get, $"api/v2/{AccountId}/Messages/{form}/Triggers/Active", nameof(SendSinglePushMessageAsync), YulsnApiVersion.V2);
    }
}
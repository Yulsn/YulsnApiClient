using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<string[]> GetActiveTriggerIdsAsync(YulsnMessageForm form) =>
            SendAsync<string[]>(HttpMethod.Get, $"api/v2/{AccountId}/Messages/{form}/TriggerIds/Active", nameof(SendSinglePushMessageAsync), YulsnApiVersion.V2);
    }
}
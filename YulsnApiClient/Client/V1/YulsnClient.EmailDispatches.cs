using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<YulsnReadEmailDispatchDto> CreateEmailDispatchAsync(YulsnCreateEmailDispatchDto yulsnCreateEmailDispatchDto) =>
            SendAsync<YulsnReadEmailDispatchDto>(new HttpRequestMessage(HttpMethod.Post, "api/v1/EmailDispatches")
            {
                Content = JsonContent(yulsnCreateEmailDispatchDto)
            });
    }
}
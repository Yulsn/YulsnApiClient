using System;
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

        public Task<string> CreateEmail2DispatchAsync(YulsnCreateEmail2DispatchDto yulsnCreateEmail2DispatchDto) =>
            SendAsync<string>(new HttpRequestMessage(HttpMethod.Post, "api/v1/Email2Dispatches")
            {
                Content = JsonContent(yulsnCreateEmail2DispatchDto)
            });
    }
}
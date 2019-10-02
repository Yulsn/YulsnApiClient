using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Net.Http;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {        
        public Task<YulsnReadEmailDispatchDto> CreateEmailDispatch(YulsnCreateEmailDispatchDto yulsnCreateEmailDispatchDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/EmailDispatches");

            request.Content = json(yulsnCreateEmailDispatchDto);

            return sendAsync<YulsnReadEmailDispatchDto>(request);
        }
    }
}
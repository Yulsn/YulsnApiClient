using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<ReadYulsnMedia> CreateMediaAsync(CreateYulsnMedia createModel) =>
            SendAsync<ReadYulsnMedia>(HttpMethod.Post, "/api/v1/Media", createModel);
    }
}

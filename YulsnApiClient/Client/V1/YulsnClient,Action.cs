using System.Collections.Generic;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<YulsnReadAction>> GetActionsAsync() =>
            SendAsync<List<YulsnReadAction>>($"/api/v1/Actions");
    }
}
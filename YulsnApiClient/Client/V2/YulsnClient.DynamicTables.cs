using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<YulsnDynamicTable>> GetDynamicTablesAsync() =>
            SendAsync<List<YulsnDynamicTable>>(HttpMethod.Get, $"api/v2/{AccountId}/DynamicTables", nameof(GetDynamicTablesAsync), YulsnApiVersion.V2);

        public Task<YulsnDynamicTable> GetDynamicTableByIdAsync(int dynamicTableId) =>
            SendAsync<YulsnDynamicTable>(HttpMethod.Get, $"api/v2/{AccountId}/DynamicTables/{dynamicTableId}", nameof(GetDynamicTableByIdAsync), YulsnApiVersion.V2);

        public Task<YulsnDynamicTable> GetDynamicTableByNameAsync(string dynamicTableName) =>
            SendAsync<YulsnDynamicTable>(HttpMethod.Get, $"api/v2/{AccountId}/DynamicTables/Name/{dynamicTableName}", nameof(GetDynamicTableByNameAsync), YulsnApiVersion.V2);

        public Task<int> CreateDynamicTableAsync(YulsnDynamicTableAddRequest model) =>
            SendAsync<int>(HttpMethod.Post, $"api/v2/{AccountId}/DynamicTables", model, nameof(CreateDynamicTableAsync), YulsnApiVersion.V2);

        public Task UpdateDynamicTableAsync(int dynamicTableId, YulsnDynamicTableUpdateRequest model) =>
            SendAsync<object>(HttpMethod.Post, $"api/v2/{AccountId}/DynamicTables/{dynamicTableId}", model, nameof(UpdateDynamicTableAsync), YulsnApiVersion.V2);
    }
}

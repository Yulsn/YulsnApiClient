using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<YulsnDynamicField>> GetDynamicFieldsAsync(TableOwner tableOwner) =>
            SendAsync<List<YulsnDynamicField>>(HttpMethod.Get, $"api/v2/{AccountId}/DynamicFields/{tableOwner}", YulsnApiVersion.V2);

        public Task<List<YulsnDynamicField>> GetDynamicFieldsAsync(int dynamicTableId) =>
            SendAsync<List<YulsnDynamicField>>(HttpMethod.Get, $"api/v2/{AccountId}/DynamicFields/DynamicTable/{dynamicTableId}", YulsnApiVersion.V2);

        public Task<YulsnDynamicField> GetDynamicFieldAsync(TableOwner tableOwner, int dynamicFieldId) =>
            SendAsync<YulsnDynamicField>(HttpMethod.Get, $"api/v2/{AccountId}/DynamicFields/{tableOwner}/{dynamicFieldId}", YulsnApiVersion.V2);

        public Task<YulsnDynamicField> GetDynamicFieldAsync(int dynamicTableId, int dynamicFieldId) =>
            SendAsync<YulsnDynamicField>(HttpMethod.Get, $"api/v2/{AccountId}/DynamicFields/DynamicTable/{dynamicTableId}/{dynamicFieldId}", YulsnApiVersion.V2);

        public Task<int> CreateDynamicFieldAsync(TableOwner tableOwner, YulsnDynamicFieldAddRequest model) =>
            SendAsync<int>(HttpMethod.Post, $"api/v2/{AccountId}/DynamicFields/{tableOwner}", model, YulsnApiVersion.V2);

        public Task<int> CreateDynamicFieldAsync(int dynamicTableId, YulsnDynamicFieldAddRequest model) =>
            SendAsync<int>(HttpMethod.Post, $"api/v2/{AccountId}/DynamicFields/DynamicTable/{dynamicTableId}", model, YulsnApiVersion.V2);

        public Task UpdateDynamicFieldAsync(TableOwner tableOwner, int dynamicFieldId, YulsnDynamicFieldUpdateRequest model) =>
            SendAsync<object>(HttpMethod.Post, $"api/v2/{AccountId}/DynamicFields/{tableOwner}/{dynamicFieldId}", model, YulsnApiVersion.V2);

        public Task UpdateDynamicFieldAsync(int dynamicTableId, int dynamicFieldId, YulsnDynamicFieldUpdateRequest model) =>
            SendAsync<object>(HttpMethod.Post, $"api/v2/{AccountId}/DynamicFields/DynamicTable/{dynamicTableId}/{dynamicFieldId}", model, YulsnApiVersion.V2);

        public Task UpdateDynamicFieldRelationAsync(TableOwner tableOwner, int dynamicFieldId, YulsnDynamicFieldRelationUpdateRequest model) =>
            SendAsync<object>(HttpMethod.Post, $"api/v2/{AccountId}/DynamicFields/{tableOwner}/{dynamicFieldId}/Relation", model, YulsnApiVersion.V2);

        public Task UpdateDynamicFieldRelationAsync(int dynamicTableId, int dynamicFieldId, YulsnDynamicFieldRelationUpdateRequest model) =>
            SendAsync<object>(HttpMethod.Post, $"api/v2/{AccountId}/DynamicFields/DynamicTable/{dynamicTableId}/{dynamicFieldId}/Relation", model, YulsnApiVersion.V2);
    }
}

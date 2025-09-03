using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<YulsnExportItem>> GetBulkExportsAsync(ExportType? type = null) =>
            SendAsync<List<YulsnExportItem>>($"api/v1/BulkExport{(type.HasValue ? $"?type={(int)type}" : null)}");

        public Task CreateBulkExportAsync(YulsnExportSettings settings) =>
            SendAsync<object>(HttpMethod.Post, "api/v1/BulkExport", settings);
    }
}
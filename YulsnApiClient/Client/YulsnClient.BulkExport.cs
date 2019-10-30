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
        public Task<List<YulsnExportItem>> GetBulkExports(ExportType? type = null) =>
            SendAsync<List<YulsnExportItem>>($"/api/v1/BulkExport{(type.HasValue ? $"?type={(int)type}" : null)}");

        public Task CreateBulkExport(YulsnExportSettings settings) =>
            SendAsync<object>(HttpMethod.Post, "api/v1/BulkExport", settings);
    }
}
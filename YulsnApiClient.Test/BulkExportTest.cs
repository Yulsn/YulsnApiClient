using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models;

namespace YulsnApiClient.Test
{
    public class BulkExportTest : IClassFixture<Setup>
    {
        private readonly YulsnClient yulsnClient;

        public BulkExportTest(Setup setup)
        {
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }

        [Fact]
        public async Task CreateBulkExport()
        {
            YulsnExportSettings model = new YulsnExportSettings
            {
                ColumnDelimiter = ",",
                CreatedDateTimeFrom = DateTimeOffset.UtcNow.AddDays(-7),
                CreatedDateTimeTo = DateTimeOffset.UtcNow.AddDays(-1),
                Type = ExportType.Events,
                ItemType = "test",
                RowDelimiter = Environment.NewLine,
                Fields = new List<string> { "Id", "Type", "SubType" },
                SuffixFileName = "Api Test"
            };

            await yulsnClient.CreateBulkExport(model);            
        }

        [Fact]
        public async Task GetBulkExports()
        {
            var exports = await yulsnClient.GetBulkExports();
            
            Assert.NotNull(exports);
            Assert.True(exports.Count > 0);

            exports = await yulsnClient.GetBulkExports(ExportType.Events);

            Assert.NotNull(exports);
            Assert.True(exports.Count > 0);
        }
    }
}

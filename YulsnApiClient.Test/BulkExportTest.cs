using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

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

            await yulsnClient.CreateBulkExportAsync(model);
        }

        [Fact]
        public async Task GetBulkExports()
        {
            var exports = await yulsnClient.GetBulkExportsAsync();

            Assert.NotNull(exports);
            Assert.True(exports.Count > 0);

            exports = await yulsnClient.GetBulkExportsAsync(ExportType.Events);

            Assert.NotNull(exports);
            Assert.True(exports.Count > 0);
        }
    }
}

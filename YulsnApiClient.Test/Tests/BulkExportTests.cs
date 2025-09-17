using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class BulkExportTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();

        [Fact]
        public async Task CreateBulkExport()
        {
            YulsnExportSettings model = new()
            {
                ColumnDelimiter = ",",
                CreatedDateTimeFrom = DateTimeOffset.UtcNow.AddDays(-7),
                CreatedDateTimeTo = DateTimeOffset.UtcNow.AddDays(-1),
                Type = ExportType.Events,
                ItemType = "test",
                RowDelimiter = Environment.NewLine,
                Fields = ["Id", "Type", "SubType"],
                SuffixFileName = "Api Test"
            };

            await _yulsnClient.CreateBulkExportAsync(model);
        }

        [Fact]
        public async Task GetBulkExports()
        {
            var exports = await _yulsnClient.GetBulkExportsAsync();

            Assert.NotNull(exports);
            Assert.True(exports.Count > 0);

            // NOTE: this can fail if there are no exports of this type yet. not sure how to handle that in a test.
            exports = await _yulsnClient.GetBulkExportsAsync(ExportType.Events);

            Assert.NotNull(exports);
            Assert.True(exports.Count > 0);
        }
    }
}

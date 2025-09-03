using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V2;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class DynamicTableTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();

        [Fact]
        public async Task GetDynamicTables()
        {
            List<YulsnDynamicTable> response = await yulsnClient.GetDynamicTablesAsync();

            Assert.NotNull(response);
            Assert.True(response.Count > 0);
        }
    }
}

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

namespace YulsnApiClient.Test
{
    public class DynamicTableEntitiesTest : IClassFixture<Setup>
    {
        private readonly IConfiguration config;
        private readonly YulsnClient yulsnClient;

        public DynamicTableEntitiesTest(Setup setup)
        {
            config = setup.ServiceProvider.GetService<IConfiguration>();
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }

        [Fact]
        public async Task GetAllExternalIds()
        {
            var list = await yulsnClient.GetAllDynamicTableEntityExternalIds(config["GetAllExternalIdTableName"]);

            Assert.True(list != null);
        }
    }
}

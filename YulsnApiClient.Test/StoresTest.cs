using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Test
{
    public class StoresTest : IClassFixture<Setup>
    {
        private readonly IConfiguration config;
        private readonly YulsnClient yulsnClient;

        public StoresTest(Setup setup)
        {
            config = setup.ServiceProvider.GetService<IConfiguration>();
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }

        [Fact]
        public async Task GetStoreById()
        {
            var store = await yulsnClient.GetStoreAsync<YulsnReadStoreDto>(1);

            Assert.True(store != null);
        }

        [Fact]
        public async Task GetStoreByNumber()
        {
            var store = await yulsnClient.GetStoreAsync<YulsnReadStoreDto>("abc");

            Assert.True(store != null);
        }
    }
}

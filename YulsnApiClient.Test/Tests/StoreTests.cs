using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class StoreTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task GetStoreById()
        {
            var store = await _yulsnClient.GetStoreAsync<YulsnReadStoreDto>(_model.ValidStoreId);

            Assert.NotNull(store);
        }

        [Fact]
        public async Task GetStoreByNumber()
        {
            var store = await _yulsnClient.GetStoreAsync<YulsnReadStoreDto>(_model.ValidStoreNumber);

            Assert.NotNull(store);
        }
    }
}

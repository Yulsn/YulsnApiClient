using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Models.V2;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class DynamicFieldTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();

        [Fact]
        public async Task Get_OK_WhenFoundByOwner_v1()
        {
            List<YulsnReadDynamicField> response = await _yulsnClient.GetDynamicFieldsAsync<YulsnReadDynamicField>(YulsnTableOwner.Contacts, id: null);

            Assert.NotNull(response);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task Get_OK_WhenFoundByOwner_v2()
        {
            List<YulsnDynamicField> response = await _yulsnClient.GetDynamicFieldsAsync(TableOwner.Contacts);

            Assert.NotNull(response);
            Assert.True(response.Count > 0);
        }
    }
}

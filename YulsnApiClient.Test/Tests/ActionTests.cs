using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class ActionTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();

        [Fact]
        public async Task GetActions()
        {
            List<YulsnReadAction> response = await yulsnClient.GetActionsAsync();

            Assert.NotNull(response);
            Assert.True(response.Count > 0);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V2;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class MessageTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();

        [Fact]
        public async Task Get_Ok_WhenFoundActiveTriggerByForm()
        {
            foreach (var form in Enum.GetValues<YulsnMessageForm>())
            {
                var response = await _yulsnClient.GetActiveTriggerIdsAsync(form);

                Assert.NotNull(response);
                Assert.True(response.Length > 0);
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class EmailDispatchReceiverTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();

        [Fact]
        public async Task Get_Ok_WhenByLastId()
        {
            HttpResponseMessage resp = await _yulsnClient.SendAsync("api/v1/EmailDispatchReceiver?lastId=");

            Assert.Fail($"resp: {resp}");

            Guid? lastId = null;

            List<YulsnEmailDispatchReceiver> response = await _yulsnClient.GetEmailDispatchReceiversAsync(lastId);

            if (response is null)
            {
                Assert.Fail($"receivers is null");
            }
            else
            {
                Assert.Fail($"receivers: {response.Count} - {response}");
            }               

            Assert.NotNull(response);
            Assert.True(response.Count > 0);
        }
    }
}

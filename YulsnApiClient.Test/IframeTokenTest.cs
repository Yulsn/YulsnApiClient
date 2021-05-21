using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;

namespace YulsnApiClient.Test
{
    public class IframeTokenTest : IClassFixture<Setup>
    {
        private readonly IConfiguration config;
        private readonly YulsnClient yulsnClient;

        public IframeTokenTest(Setup setup)
        {
            config = setup.ServiceProvider.GetService<IConfiguration>();
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }

        [Fact]
        public async Task GetToken()
        {
            //var iframeToken = await yulsnClient.GetIframeTokenAsync("4145dde6-81a5-49dc-8b44-b0530291e8cd");
            //Assert.NotNull(iframeToken);
        }
    }
}

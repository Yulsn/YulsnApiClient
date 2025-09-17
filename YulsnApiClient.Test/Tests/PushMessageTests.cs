using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V2;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class PushMessageTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task Post_Success_WhenByContactId()
        {
            YulsnSendMessageToContactRequest request = new()
            {
                ContactId = _model.ValidContactId,
                TriggerId = _model.ValidPushMessageTriggerId,
            };

            YulsnSendMessageResponse response = await _yulsnClient.SendSinglePushMessageAsync(request);

            Assert.NotNull(response);
            Assert.NotEqual(default, response.DispatchId);
        }
    }
}

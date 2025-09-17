using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class Email2DispatchTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task Post_Success_WhenByContactId()
        {
            string triggerId = _model.ValidEmail2MessageTriggerId;
            int contactId = _model.ValidContactId;

            string response = await _yulsnClient.SendSingleEmail2DispatchAsync(triggerId, contactId);

            string message = JsonConvert.DeserializeObject<string>(response);

            Assert.Equal("A new dispatch is successfully queued, but an id cannot be returned.", message);
        }
    }
}

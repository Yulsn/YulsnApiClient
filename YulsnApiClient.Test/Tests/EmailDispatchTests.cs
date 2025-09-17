using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class EmailDispatchTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task CreateEmailDispatch()
        {
            YulsnCreateEmailDispatchDto model = new()
            {
                EmailCampaignId = _model.ValidEmailCampaignId,
                ContactId = _model.ValidContactId,
                IsTest = true
            };

            var emailDispatch = await _yulsnClient.CreateEmailDispatchAsync(model);

            Assert.Equal(model.EmailCampaignId, emailDispatch?.EmailCampaignId ?? 0);
        }
    }
}

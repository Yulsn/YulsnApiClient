using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models;

namespace YulsnApiClient.Test
{
    public class EmailDispatchesTest : IClassFixture<Setup>
    {
        private readonly YulsnClient yulsnClient;

        public EmailDispatchesTest(Setup setup)
        {
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }

        [Fact]
        public async Task CreateEmailDispatch()
        {
            YulsnCreateEmailDispatchDto model = new YulsnCreateEmailDispatchDto
            {
                EmailCampaignId = 1,
                ContactId = 1,
                IsTest = true
            };

            var emailDispatch = await yulsnClient.CreateEmailDispatch(model);

            Assert.Equal(model.EmailCampaignId, emailDispatch?.EmailCampaignId ?? 0);
        }
    }
}

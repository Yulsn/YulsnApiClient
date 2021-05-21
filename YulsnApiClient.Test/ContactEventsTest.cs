using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models;

namespace YulsnApiClient.Test
{
    public class ContactEventsTest : IClassFixture<Setup>
    {
        private readonly IConfiguration config;
        private readonly YulsnClient yulsnClient;

        public ContactEventsTest(Setup setup)
        {
            config = setup.ServiceProvider.GetService<IConfiguration>();
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }

        [Fact]
        public async Task CreateContactEvent()
        {
            List<YulsnCreateContactEvent> models = new List<YulsnCreateContactEvent>{
                new YulsnCreateContactEvent
                {
                    Type = "test",
                    SubType = "test",
                    ParentId = "parent",
                    ParentType = "test",
                    UniqueExtId = Guid.NewGuid().ToString(),
                    ContactId = 1
                },
                new YulsnCreateContactEvent
                {
                    Type = "test",
                    SubType = "test",
                    ParentId = "parent",
                    ParentType = "test",
                    UniqueExtId = Guid.NewGuid().ToString(),
                    ContactId = 1
                }
            };

            await yulsnClient.CreateContactEventAsync(models, true);
        }

        [Fact]
        public async Task GetContactEvents()
        {
            var list = await yulsnClient.GetContactEventsAsync<YulsnReadContactEvent>("test", 0);

            Assert.NotNull(list);

            list = await yulsnClient.GetContactEventsAsync<YulsnReadContactEvent>(
                "test",
                "test",
                DateTimeOffset.UtcNow.AddMonths(-1),
                DateTimeOffset.UtcNow.AddHours(1),
                1);

            Assert.NotNull(list);
        }
    }
}

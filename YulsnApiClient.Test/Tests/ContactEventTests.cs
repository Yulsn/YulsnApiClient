using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class ContactEventTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task CreateContactEvent()
        {
            YulsnCreateContactEvent yulsnEvent = new()
            {
                Type = "test",
                SubType = "sub_test",
                ParentId = "integration_test",
                ParentType = "xunit",
                UniqueExtId = Guid.NewGuid().ToString(),
                ContactId = _model.ValidContactId
            };

            {
                await _yulsnClient.CreateContactEventAsync(
                    yulsnCreateContactEvents: [yulsnEvent],
                    ignoreUnknownContacts: true);
            }

            {
                List<YulsnReadContactEvent> list = await _yulsnClient.GetContactEventsAsync<YulsnReadContactEvent>(type: yulsnEvent.Type, lastId: 0);

                Assert.NotNull(list);
                Assert.NotEmpty(list);
            }

            {
                List<YulsnReadContactEvent> list = await _yulsnClient.GetContactEventsAsync<YulsnReadContactEvent>(
                    type: yulsnEvent.Type,
                    subtype: yulsnEvent.SubType,
                    from: DateTimeOffset.UtcNow.AddDays(-1),
                    to: DateTimeOffset.UtcNow.AddHours(1),
                    contactId: yulsnEvent.ContactId);

                Assert.NotNull(list);
                Assert.NotEmpty(list);
                Assert.Contains(list, o => o.UniqueExtId == yulsnEvent.UniqueExtId);
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Models.V2;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class PushTokenTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task Get_Success_WhenManage()
        {
            int contactId = _model.ValidContactId;
            string value = $"xunit_{Guid.NewGuid()}";

            int id = default;

            // add new push token
            {
                YulsnPushTokenPostResponse response = await _yulsnClient.AddPushTokenToContactAsync(new YulsnPushTokenPostRequest
                {
                    ContactId = contactId,
                    Value = value,
                });

                Assert.NotNull(response);
                Assert.NotEqual(default, response.Id);

                id = response.Id;
            }

            // get push token
            {
                List<YulsnPushToken> response = await _yulsnClient.GetPushTokensAsync(contactId);

                Assert.NotNull(response);
                Assert.Contains(response, x => x.Id == id && x.Value == value);
            }

            // delete new push token
            {
                await _yulsnClient.DeletePushTokenFromContactAsync(new YulsnPushTokenDeleteRequest
                {
                    ContactId = contactId,
                    Value = value,
                });
            }

            // check if the push token is deleted
            {
                List<YulsnPushToken> response = await _yulsnClient.GetPushTokensAsync(contactId);

                Assert.NotNull(response);
                Assert.DoesNotContain(response, x => x.Id == id);
            }
        }
    }
}

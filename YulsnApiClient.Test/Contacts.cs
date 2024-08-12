using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Test
{
    public class Contacts : IClassFixture<Setup>
    {
        private readonly IConfiguration config;
        private readonly YulsnClient yulsnClient;
        public Contacts(Setup setup)
        {
            config = setup.ServiceProvider.GetService<IConfiguration>();
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }

        [Fact]
        public async Task GetContactById()
        {
            var contact = await yulsnClient.GetContactByIdAsync<YulsnContact>(1);
            Assert.NotNull(contact);
        }

        [Fact]
        public async Task CreateDeleteContact()
        {
            var contact = await yulsnClient.CreateContactAsync<YulsnCreateContact, YulsnContact>(new YulsnCreateContact
            {
                Email = "test2@test.invalid",
                RegistrationDateTime = DateTimeOffset.Now,
                RegistrationIp = "1.1.1.1",
                RegistrationSource = "ApiClientTest",
            });
            Assert.NotNull(contact);
            await yulsnClient.DeleteContactAsync(contact.Id);
        }

        [Fact]
        public async Task SetContactPassword()
        {
            await yulsnClient.SetContactPasswordAsync(1, "testpassword");
        }

        [Fact]
        public async Task LoginContact()
        {
            var contact = await yulsnClient.LoginContactAsync<YulsnContact>(1, new YulsnLoginContact { Password = "testpassword", Ip = "1.1.1.1", Source = "UnitTest" });
            try
            {
                var contactWrong = await yulsnClient.LoginContactAsync<YulsnContact>(1, new YulsnLoginContact { Password = "wrongpassword", Ip = "1.1.1.1", Source = "UnitTest" });
                Assert.NotNull(contact);
                Assert.Null(contactWrong);
            }
            catch (HttpRequestException e)
            {
                if (!e.Message.Contains("422"))
                    throw;
            }
        }
    }
}

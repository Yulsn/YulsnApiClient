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
    }


}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class ContactTests(Setup setup) : IClassFixture<Setup>
    {
        private class YulsnTestContact : YulsnContact
        {
            public string Email { get; set; }
        }

        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task GetContactById()
        {
            var contact = await _yulsnClient.GetContactByIdAsync<YulsnContact>(_model.ValidContactId);

            Assert.NotNull(contact);
        }

        [Fact]
        public async Task CreateDeleteContact()
        {
            string salt = $"{Guid.NewGuid():N}";
            string email = $"test+{salt}@integration.xunit";

            YulsnCreateContact newContact = new()
            {
                Email = email,
                RegistrationDateTime = DateTimeOffset.Now,
                RegistrationIp = "1.1.1.1",
                RegistrationSource = "YulsnApiClient.Test",
            };

            YulsnTestContact contact;

            {
                contact = await _yulsnClient.CreateContactAsync<YulsnCreateContact, YulsnTestContact>(newContact);

                Assert.NotNull(contact);
                Assert.Equal(email, contact.Email);
            }

            {
                await _yulsnClient.DeleteContactAsync(contact.Id);

                var deletedContact = await _yulsnClient.GetContactByIdAsync<YulsnContact>(contact.Id);

                Assert.Null(deletedContact);
            }
        }

        [Fact]
        public async Task SetContactPasswordAndLogin()
        {
            string newPassword = Guid.NewGuid().ToString("N");
            string wrongPassword = Guid.NewGuid().ToString("N");
            string validIp = "1.1.1.1";
            string source = "XUnitTest";

            await _yulsnClient.SetContactPasswordAsync(_model.ValidContactId, newPassword);

            YulsnContact contact = await _yulsnClient.LoginContactAsync<YulsnContact>(_model.ValidContactId, new YulsnLoginContact
            {
                Password = newPassword,
                Ip = validIp,
                Source = source
            });

            Assert.NotNull(contact);
            Assert.Equal(_model.ValidContactId, contact.Id);

            try
            {
                var contactWrong = await _yulsnClient.LoginContactAsync<YulsnContact>(_model.ValidContactId, new YulsnLoginContact
                {
                    Password = wrongPassword,
                    Ip = validIp,
                    Source = source
                });

                Assert.Null(contactWrong);
            }
            catch (HttpRequestException e)
            {
                // Password is not valid
                if (!e.Message.Contains("422"))
                    throw;
            }
        }
    }
}

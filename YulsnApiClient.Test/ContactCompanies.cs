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
    public class ContactCompatniesTest : IClassFixture<Setup>
    {
        private readonly IConfiguration config;
        private readonly YulsnClient yulsnClient;

        public ContactCompatniesTest(Setup setup)
        {
            config = setup.ServiceProvider.GetService<IConfiguration>();
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }
        [Fact]
        public async Task GetCompanies()
        {
            var companies = await yulsnClient.GetContactCompaniesAsync<YulsnReadContactCompany>();
            Assert.NotNull(companies);
        }
        [Fact]
        public async Task GetCompany()
        {
            var company = await yulsnClient.GetContactCompanyAsync<YulsnReadContactCompany>(1);
            Assert.NotNull(company);
        }
        [Fact]
        public async Task GetCompaniesByPrimaryContactEmail()
        {
            var company = await yulsnClient.GetContactCompaniesAsync<YulsnReadContactCompany>("test@test.invalid");
            Assert.NotNull(company);
        }
        [Fact]
        public async Task GetCompanyContacts()
        {
            var contacts = await yulsnClient.GetContactCompanyContactsAsync<YulsnContact>(1);
        }
        [Fact]
        public async Task AddRemoveContactToCompany()
        {
            await yulsnClient.ContactCompanyAddContact(1, 1);
            await yulsnClient.ContactCompanyRemoveContact(1, 1);
        }
        [Fact]
        public async Task CreateDeleteCompany()
        {
            //var company = await yulsnClient.CreateContactCompanyAsync<YulsnContactCompany>(new YulsnCreateContactCompany { 
            //   Name="Test",
            //   PrimaryContactId=1,
            //   ContactCompanyId=null,
            //   EmailDomains=null,
            //   Logo=null,
            //   Number=null,
            //});
            //Assert.NotNull(company);
        }
    }
}

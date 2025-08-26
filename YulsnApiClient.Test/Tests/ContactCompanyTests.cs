using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class ContactCompanyTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task GetCompanies()
        {
            var companies = await yulsnClient.GetContactCompaniesAsync<YulsnReadContactCompany>();

            Assert.NotNull(companies);
        }

        [Fact]
        public async Task GetCompany()
        {
            var company = await yulsnClient.GetContactCompanyAsync<YulsnReadContactCompany>(_model.ValidContactCompanyId);

            Assert.NotNull(company);
        }

        [Fact]
        public async Task GetCompaniesByPrimaryContactEmail()
        {
            var company = await yulsnClient.GetContactCompaniesAsync<YulsnReadContactCompany>(_model.ValidContactCompanyPrimaryContactEmail);

            Assert.NotNull(company);
        }

        [Fact]
        public async Task GetCompanyContacts()
        {
            var contacts = await yulsnClient.GetContactCompanyContactsAsync<YulsnContact>(_model.ValidContactCompanyId);

            Assert.NotNull(contacts);
            Assert.NotEmpty(contacts);
        }

        [Fact]
        public async Task AddRemoveContactToCompany()
        {
            int contactCompanyId = _model.ValidContactCompanyId;
            int newContactId = _model.UnlinkedContactCompanyContactId;

            {
                List<YulsnContact> contacts = await yulsnClient.GetContactCompanyContactsAsync<YulsnContact>(contactCompanyId);

                Assert.DoesNotContain(contacts, c => c.Id == newContactId);
            }

            {
                await yulsnClient.ContactCompanyAddContactAsync(contactCompanyId, newContactId);

                List<YulsnContact> contacts = await yulsnClient.GetContactCompanyContactsAsync<YulsnContact>(contactCompanyId);

                Assert.Contains(contacts, c => c.Id == newContactId);
            }

            {
                await yulsnClient.ContactCompanyRemoveContactAsync(contactCompanyId, newContactId);

                List<YulsnContact> contacts = await yulsnClient.GetContactCompanyContactsAsync<YulsnContact>(contactCompanyId);

                Assert.DoesNotContain(contacts, c => c.Id == newContactId);
            }
        }
    }
}

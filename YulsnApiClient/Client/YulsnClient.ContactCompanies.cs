using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Net.Http;
using System.Collections.Generic;
using System;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<T>> GetContactCompaniesAsync<T>() where T : YulsnReadContactCompany =>
            SendAsync<List<T>>($"/api/v1/ContactCompanies");

        public Task<T> GetContactCompanyAsync<T>(int id) where T : YulsnReadContactCompany =>
            SendAsync<T>($"/api/v1/ContactCompanies/{id}");
        /// <summary>
        /// Gets a list of Contacts associated with the ContactCompany. If the ContactCompany has no Contacts throws an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">Company Id</param>
        /// <returns></returns>
        public Task<List<T>> GetContactCompanyContactsAsync<T>(int id) where T : YulsnContact =>
            SendAsync<List<T>>($"/api/v1/ContactCompanies/{id}/Contacts");
        public Task<R> CreateContactCompanyAsync<T,R>(T company) where T : YulsnCreateContactCompany where R: YulsnReadContactCompany=>
            SendAsync<R>(HttpMethod.Post, $"/api/v1/ContactCompanies", company);
        public Task<List<T>> GetContactCompaniesAsync<T>(string primaryContactEmail) where T : YulsnReadContactCompany =>
            SendAsync<List<T>>($"/api/v1/ContactCompanies?primaryContactEmail={Uri.EscapeDataString(primaryContactEmail)}");
        /// <summary>
        /// Associates a Contact with a ContactCompany
        /// </summary>
        /// <param name="id">Company Id</param>
        /// <param name="contactId">Contacts Id</param>
        /// <returns></returns>
        public Task ContactCompanyAddContactAsync(int id, int contactId) =>
            SendAsync<object>(HttpMethod.Post, $"/api/v1/ContactCompanies/{id}/AddContact",new {ContactId=contactId });
        public Task ContactCompanyRemoveContactAsync(int id, int contactId) =>
            SendAsync<object>(HttpMethod.Post, $"/api/v1/ContactCompanies/{id}/RemoveContact", new { ContactId = contactId });
    }
}

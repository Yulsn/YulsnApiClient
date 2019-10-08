using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Net.Http;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>
        /// Will return contact by its secret or null if the contact was not found.
        /// </summary>
        /// <param name="secret">Contact secret</param>
        /// <param name="ip">If provided - it will be used for brute force protection</param>
        /// <returns></returns>
        public Task<T> GetContactBySecret<T>(string secret, string ip = null) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?secret={secret + (ip != null ? $"&ip={ip}" : "")}");

        public Task<T> GetContactByEmail<T>(string email) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?email={email}");

        public Task<T> GetContactByPhone<T>(string phone) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?phone={phone}");

        public Task DeleteContact(int contactId)
        {
            string url = "/api/v1/Contacts/" + contactId;

            return SendAsync<object>(HttpMethod.Delete, url);
        }
    }
}
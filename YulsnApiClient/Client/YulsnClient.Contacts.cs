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

        public Task<T> UpdateContact<T>(int id, Dictionary<string, object> updateFields) where T : YulsnContact =>
            SendAsync<T>(new HttpMethod("PATCH"), $"api/v1/Contacts/{id}", updateFields);

        public Task DeleteContact(int id) =>
            SendAsync<object>(HttpMethod.Delete, $"/api/v1/Contacts/{id}");
    }
}
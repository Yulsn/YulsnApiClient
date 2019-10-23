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

        public Task<T> GetContactByIdAsync<T>(int id) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts/{id}");
        /// <summary>
        /// Will return contact by its secret or null if the contact was not found.
        /// </summary>
        /// <param name="secret">Contact secret</param>
        /// <param name="ip">If provided - it will be used for brute force protection</param>
        /// <returns></returns>
        public Task<T> GetContactBySecretAsync<T>(string secret, string ip = null) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?secret={WebUtility.UrlEncode(secret) + (ip != null ? $"&ip={WebUtility.UrlEncode(ip)}" : "")}");

        public Task<T> GetContactByEmailAsync<T>(string email) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?email={WebUtility.UrlEncode(email)}");

        public Task<T> GetContactByPhoneAsync<T>(string phone) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?phone={WebUtility.UrlEncode(phone)}");

        public Task<T> UpdateContactAsync<T>(int id, Dictionary<string, object> updateFields) where T : YulsnContact =>
            SendAsync<T>(new HttpMethod("PATCH"), $"api/v1/Contacts/{id}", updateFields);

        public Task DeleteContactAsync(int id) =>
            SendAsync<object>(HttpMethod.Delete, $"/api/v1/Contacts/{id}");
        public Task<R> CreateContactAsync<T,R>(T contact) where T : YulsnCreateContact where R: YulsnContact  =>
            SendAsync<R>(HttpMethod.Post, $"/api/v1/Contacts", contact);
    }
}
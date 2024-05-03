using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<T> GetContactByIdAsync<T>(int id) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts/{id}", nameof(GetContactByIdAsync));

        /// <summary>
        /// Get contact as Dictionary
        /// </summary>
        /// <typeparam name="T">The type of the Value part in the returned dictionary. Key is of the type string</typeparam>
        /// <param name="id">Id of the contact</param>
        /// <returns></returns>
        public Task<Dictionary<string, T>> GetContactAsDictionaryByIdAsync<T>(int id) =>
            SendAsync<Dictionary<string, T>>($"/api/v1/Contacts/{id}", nameof(GetContactAsDictionaryByIdAsync));

        /// <summary>
        /// Will return contact by its secret or null if the contact was not found.
        /// </summary>
        /// <param name="secret">Contact secret</param>
        /// <param name="ip">If provided - it will be used for brute force protection</param>
        /// <returns></returns>
        public Task<T> GetContactBySecretAsync<T>(string secret, string ip = null) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?secret={WebUtility.UrlEncode(secret) + (ip != null ? $"&ip={WebUtility.UrlEncode(ip)}" : "")}", nameof(GetContactBySecretAsync));

        public Task<T> GetContactByEmailAsync<T>(string email) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?email={WebUtility.UrlEncode(email)}", nameof(GetContactByEmailAsync));

        public Task<T> GetContactByPhoneAsync<T>(string phone) where T : YulsnContact =>
            SendAsync<T>($"/api/v1/Contacts?phone={WebUtility.UrlEncode(phone)}", nameof(GetContactByPhoneAsync));

        /// <summary>Will return a contact by a field and value</summary>
        /// <param name="field">Name of the field</param>
        /// <param name="value">Value of the field</param>
        /// <returns></returns>
        public Task<List<T>> GetContactByFieldValueAsync<T>(string field, string value) where T : YulsnContact =>
            SendAsync<List<T>>($"api/v1/Contacts?field={WebUtility.UrlEncode(field)}&value={WebUtility.UrlEncode(value)}", nameof(GetContactByFieldValueAsync));

        public Task<T> UpdateContactAsync<T>(int id, Dictionary<string, object> updateFields) where T : YulsnContact =>
            SendAsync<T>(new HttpMethod("PATCH"), $"api/v1/Contacts/{id}", updateFields, nameof(UpdateContactAsync));

        public Task<YulsnUpdateContactsResult> UpdateContactsAsync(YulsnUpdateContacts updateContacts) =>
            SendAsync<YulsnUpdateContactsResult>(new HttpMethod("PATCH"), $"api/v1/Contacts", updateContacts, nameof(UpdateContactsAsync));

        public Task DeleteContactAsync(int id) =>
            SendAsync<object>(HttpMethod.Delete, $"/api/v1/Contacts/{id}", nameof(DeleteContactAsync));

        public Task<R> CreateContactAsync<T, R>(T contact) where T : YulsnCreateContact where R : YulsnContact =>
            SendAsync<R>(HttpMethod.Post, $"/api/v1/Contacts", contact, nameof(CreateContactAsync));

        public Task<T> LoginContactAsync<T>(int id, YulsnLoginContact loginContact) where T : YulsnContact =>
            SendAsync<T>(HttpMethod.Post, $"/api/v1/Contacts/{id}/login", loginContact, nameof(LoginContactAsync));

        public Task SetContactPasswordAsync(int id, string newPassword) =>
            SendAsync<object>(HttpMethod.Post, $"/api/v1/Contacts/{id}/SetPassword", new { NewPassword = newPassword }, nameof(SetContactPasswordAsync));
    }
}
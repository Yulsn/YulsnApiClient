using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<YulsnPushToken>> GetPushTokensAsync(int contactId) =>
            SendAsync<List<YulsnPushToken>>(HttpMethod.Get, $"api/v2/{AccountId}/PushTokens/Contact/{contactId}", YulsnApiVersion.V2);

        /// <summary>
        /// Adds a new push token to a contact
        /// </summary>
        public Task<YulsnPushTokenPostResponse> AddPushTokenToContactAsync(YulsnPushTokenPostRequest body) =>
            SendAsync<YulsnPushTokenPostResponse>(HttpMethod.Post, $"api/v2/{AccountId}/PushTokens", body, YulsnApiVersion.V2);

        /// <summary>
        /// Deletes an existing push token from a contact
        /// </summary>
        public Task DeletePushTokenFromContactAsync(YulsnPushTokenDeleteRequest body) =>
            SendAsync<object>(HttpMethod.Delete, $"api/v2/{AccountId}/PushTokens", body, YulsnApiVersion.V2);
    }
}

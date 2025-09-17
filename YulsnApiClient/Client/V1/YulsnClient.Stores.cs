using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<T> GetStoreAsync<T>(int storeId) where T : YulsnReadStoreDto =>
            SendAsync<T>($"api/v1/Stores/{storeId}");

        public Task<T> GetStoreAsync<T>(string number) where T : YulsnReadStoreDto =>
            SendAsync<T>($"api/v1/Stores/?number={number}");

        public Task<T> CreateStoreAsync<T, R>(R store) where T : YulsnReadStoreDto where R : YulsnCreateStoreDto =>
            SendAsync<T>(HttpMethod.Post, $"api/v1/Stores", store);
    }
}
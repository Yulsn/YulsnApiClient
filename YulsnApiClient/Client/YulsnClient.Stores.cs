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
        public Task<T> GetStore<T>(int storeId) where T : YulsnReadStoreDto =>
            SendAsync<T>($"/api/v1/Stores/{storeId}");

        public Task<T> GetStore<T>(string number) where T : YulsnReadStoreDto =>
            SendAsync<T>($"/api/v1/Stores/?number={number}");

        public Task<T> CreateStore<T, R>(R store) where T : YulsnReadStoreDto where R : YulsnCreateStoreDto =>
            SendAsync<T>(HttpMethod.Post, $"/api/v1/Stores", store);
    }
}
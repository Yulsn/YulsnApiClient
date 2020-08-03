using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        public Task<T> GetOrderd<T>(int orderId) where T : YulsnReadOrderDto =>
            SendAsync<T>($"/api/v1/Orders/{orderId}");

        public Task<List<T>> GetOrdersByContactId<T>(int contactId) where T : YulsnReadOrderDto =>
           SendAsync<List<T>>($"/api/v1/Orders?contactid={contactId}");

        public Task<List<T>> GetOrdersByExtOrderId<T>(string extOrderId) where T : YulsnReadOrderDto =>
            SendAsync<List<T>>($"/api/v1/Orders?extOrderId={extOrderId}");

        public Task<T> CreateOrder<T,R>(R order) where T : YulsnReadOrderDto where R : YulsnCreateOrderDto =>
          SendAsync<T>(HttpMethod.Post, $"/api/v1/Orders", order);

        /// <summary>
        /// Update order - only provided fields will be changed
        /// </summary>
        public Task<T> UpdateOrder<T>(int orderId, Dictionary<string, object> updateFields) where T : YulsnReadOrderDto =>
               SendAsync<T>(new HttpMethod("PATCH"), $"api/v1/Orders/{orderId}", updateFields);
    }
}

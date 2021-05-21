using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        public Task<T> GetOrderAsync<T>(int orderId) where T : YulsnReadOrderDto =>
            SendAsync<T>($"/api/v1/Orders/{orderId}");

        public Task<List<T>> GetOrdersByContactIdAsync<T>(int contactId) where T : YulsnReadOrderDto =>
            SendAsync<List<T>>($"/api/v1/Orders?contactid={contactId}");

        public Task<List<T>> GetOrdersByExtOrderIdAsync<T>(string extOrderId) where T : YulsnReadOrderDto =>
            SendAsync<List<T>>($"/api/v1/Orders?extOrderId={extOrderId}");

        public Task<T> CreateOrderAsync<T, R>(R order) where T : YulsnReadOrderDto where R : YulsnCreateOrderDto =>
            SendAsync<T>(HttpMethod.Post, $"/api/v1/Orders", order);

        /// <summary>Update order - only provided fields will be changed</summary>
        public Task<T> UpdateOrderAsync<T>(int orderId, Dictionary<string, object> updateFields) where T : YulsnReadOrderDto =>
            SendAsync<T>(new HttpMethod("PATCH"), $"api/v1/Orders/{orderId}", updateFields);

        public Task<List<T>> SearchOrdersAsync<T>(List<YulsnSearchFieldDto> yulsnSearchFileldDtos) where T : YulsnReadOrderDto =>
            SendAsync<List<T>>(new HttpMethod("SEARCH"), $"api/v1/Orders", yulsnSearchFileldDtos);
    }
}
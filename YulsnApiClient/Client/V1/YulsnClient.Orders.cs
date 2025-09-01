using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        public Task<RO> GetOrderAsync<RO, RL>(int orderId) where RO : YulsnReadOrderDto<RL> where RL : YulsnReadOrderLineDto =>
            SendAsync<RO>($"/api/v1/Orders/{orderId}");

        public Task<List<RO>> GetOrdersByContactIdAsync<RO, RL>(int contactId, string orderType = null, bool? withOrderLines = null) where RO : YulsnReadOrderDto<RL> where RL : YulsnReadOrderLineDto =>
            SendAsync<List<RO>>($"/api/v1/Orders?contactid={contactId}{(orderType != null ? $"&orderType={orderType}" : null)}{(withOrderLines != null ? $"&withOrderLines={withOrderLines}" : null)}");

        public Task<List<RO>> GetOrdersByExtOrderIdAsync<RO, RL>(string extOrderId) where RO : YulsnReadOrderDto<RL> where RL : YulsnReadOrderLineDto =>
            SendAsync<List<RO>>($"/api/v1/Orders?extOrderId={extOrderId}");

        public Task<RO> CreateOrderAsync<RO, RL, CO, CL>(CO order) where RO : YulsnReadOrderDto<RL> where RL : YulsnReadOrderLineDto where CO : YulsnCreateOrderDto<CL> where CL : YulsnCreateOrderLineDto =>
            SendAsync<RO>(HttpMethod.Post, "/api/v1/Orders", order);

        /// <summary>Update order - only provided fields will be changed</summary>
        public Task<RO> UpdateOrderAsync<RO, RL>(int orderId, Dictionary<string, object> updateFields) where RO : YulsnReadOrderDto<RL> where RL : YulsnReadOrderLineDto =>
            SendAsync<RO>(new HttpMethod("PATCH"), $"api/v1/Orders/{orderId}", updateFields);

        public Task<List<RO>> SearchOrdersAsync<RO, RL>(List<YulsnSearchFieldDto> yulsnSearchFileldDtos) where RO : YulsnReadOrderDto<RL> where RL : YulsnReadOrderLineDto =>
            SendAsync<List<RO>>(new HttpMethod("SEARCH"), "/api/v1/Orders", yulsnSearchFileldDtos);

        public Task<Dictionary<string, object>> GetOrderAsDictionaryAsync(int orderId) =>
            SendAsync<Dictionary<string, object>>($"/api/v1/Orders/{orderId}");

        public Task<List<Dictionary<string, object>>> GetOrdersAsDictionaryByContactIdAsync(int contactId, string orderType = null, bool? withOrderLines = null)
        {
            string endpoint = $"/api/v1/Orders?contactid={contactId}";

            if (!string.IsNullOrWhiteSpace(orderType))
                endpoint += $"&orderType={orderType}";

            if (withOrderLines.HasValue)
                endpoint += $"&withOrderLines={withOrderLines}";

            return SendAsync<List<Dictionary<string, object>>>(endpoint);
        }

        public Task<List<Dictionary<string, object>>> GetOrdersAsDictionaryByExtOrderIdAsync(string extOrderId) =>
            SendAsync<List<Dictionary<string, object>>>($"/api/v1/Orders?extOrderId={extOrderId}");

        public Task<List<string>> GetOrderTypesAsync() =>
            SendAsync<List<string>>("/api/v1/Orders/types");

        public Task<List<Dictionary<string, object>>> SearchOrdersAsDictionaryAsync(List<YulsnSearchFieldDto> yulsnSearchFileldDtos) =>
            SendAsync<List<Dictionary<string, object>>>(new HttpMethod("SEARCH"), "/api/v1/Orders", yulsnSearchFileldDtos);

        public Task<Dictionary<string, object>> CreateOrderAsDictionaryAsync(Dictionary<string, object> order) =>
            SendAsync<Dictionary<string, object>>(HttpMethod.Post, "/api/v1/Orders", order);

        public Task<Dictionary<string, object>> CreateOrderLineAsDictionaryAsync(int orderId, Dictionary<string, object> orderLine) =>
            SendAsync<Dictionary<string, object>>(HttpMethod.Post, $"/api/v1/Orders/{orderId}/Lines", orderLine);

        public Task<Dictionary<string, object>> UpdateOrderAsDictionaryAsync(int orderId, Dictionary<string, object> requestContent) =>
            SendAsync<Dictionary<string, object>>(new HttpMethod("PATCH"), $"/api/v1/Orders/{orderId}", requestContent);

        public Task<Dictionary<string, object>> UpdateOrderLineAsDictionaryAsync(int orderId, int orderLineId, Dictionary<string, object> requestContent) =>
            SendAsync<Dictionary<string, object>>(new HttpMethod("PATCH"), $"/api/v1/Orders/{orderId}/Lines/{orderLineId}", requestContent);

        public Task<Dictionary<string, object>> DeleteOrderAsDictionaryAsync(int orderId) =>
            SendAsync<Dictionary<string, object>>(HttpMethod.Delete, $"/api/v1/Orders/{orderId}");

        public Task<Dictionary<string, object>> DeleteOrderLineAsDictionaryAsync(int orderId, int orderLineId) =>
            SendAsync<Dictionary<string, object>>(HttpMethod.Delete, $"/api/v1/Orders/{orderId}/Lines/{orderLineId}");
    }
}
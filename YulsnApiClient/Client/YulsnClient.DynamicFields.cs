using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        /// <summary>Retrieve collection of dynamic fields by owner</summary>
        /// <typeparam name="T">Object of type YulsnReadDynamicField</typeparam>
        /// <param name="owner"></param>
        /// <param name="id">Must be set to dynamic table id, if owner is DynamicTable</param>
        public Task<List<T>> GetDynamicFieldsAsync<T>(YulsnTableOwner owner, int? id) where T : YulsnReadDynamicField
        {
            if (id.HasValue)
                return SendAsync<List<T>>(HttpMethod.Get, $"api/v1/DynamicFields/{id}?owner={owner}");
            else
                return SendAsync<List<T>>(HttpMethod.Get, $"api/v1/DynamicFields?owner={owner}");
        }
    }
}

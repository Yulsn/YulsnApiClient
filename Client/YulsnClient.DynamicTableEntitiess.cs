using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Linq;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public async Task<List<string>> GetAllDynamicTableEntityExternalIds(string tableName)
        {
            int lastId = 0;
            int take = 1000;

            List<string> retVal = new List<string>();
            List<YulsnDynamicTableEntity> result = new List<YulsnDynamicTableEntity>();

            do
            {
                string url = $"api/v1/table/{tableName}?lastId={lastId}&take={take}";

                using (var response = await httpClient.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();

                    result = JsonConvert.DeserializeObject<List<YulsnDynamicTableEntity>>(await response.Content.ReadAsStringAsync());

                    retVal.AddRange(result.Select(o => o.ExternalId));
                    lastId = result.Max(o => o.Id);
                }
            } while (result.Count == take);

            return retVal;
        }
    }
}
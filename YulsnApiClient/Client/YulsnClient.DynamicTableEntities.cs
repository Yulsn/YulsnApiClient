using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Linq;
using System.Net.Http;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public async Task<List<string>> GetAllDynamicTableEntityExternalIds(string tableName)
        {
            int lastId = 0;
            int take = 1000;

            List<string> retVal = new List<string>();
            List<YulsnDynamicTableEntity> result = null;

            do
            {
                string url = $"api/v1/table/{tableName}?lastId={lastId}&take={take}";

                result = await sendAsync<List<YulsnDynamicTableEntity>>(new HttpRequestMessage(HttpMethod.Get, url));

                retVal.AddRange(result.Select(o => o.ExternalId));

                if (result.Count > 0)
                    lastId = result.Max(o => o.Id);

            } while (result.Count == take);

            return retVal;
        }
    }
}
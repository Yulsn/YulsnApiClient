using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<YulsnPoint> GetPointByIdAsync(int id) =>
            SendAsync<YulsnPoint>(HttpMethod.Get, $"api/v2/{AccountId}/Points/{id}", YulsnApiVersion.V2);

        public Task<List<YulsnPoint>> GetPointsPageAsync(int lastId, string type = null, int? take = null)
        {
            string url = $"api/v2/{AccountId}/Points/Page/{lastId}";

            if (type != null)
                url += $"?type={type}";

            if (take.HasValue)
                url += (type != null ? "&" : "?") + $"take={take.Value}";

            return SendAsync<List<YulsnPoint>>(HttpMethod.Get, url, YulsnApiVersion.V2);
        }

        public Task<List<YulsnPoint>> GetPointsAsync(
            int? contactId,
            string sourceId,
            string type,
            DateTimeOffset? datetimeFrom,
            DateTimeOffset? datetimeTo,
            int? contactCompanyId,
            int? storeId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (contactId.HasValue)
                parameters["contactId"] = contactId.Value.ToString();

            if (!string.IsNullOrEmpty(sourceId))
                parameters["sourceId"] = sourceId;

            if (!string.IsNullOrEmpty(type))
                parameters["type"] = type;

            if (datetimeFrom.HasValue)
                parameters["datetimeFrom"] = HttpUtility.UrlEncode(((DateTimeOffset)datetimeFrom).ToString("O"));

            if (datetimeTo.HasValue)
                parameters["datetimeTo"] = HttpUtility.UrlEncode(((DateTimeOffset)datetimeTo).ToString("O"));

            if (contactCompanyId.HasValue)
                parameters["contactCompanyId"] = contactCompanyId.Value.ToString();

            if (storeId.HasValue)
                parameters["storeId"] = storeId.Value.ToString();

            string url = null;

            foreach (var parameter in parameters)
            {
                if (url is null)
                    url = $"api/v2/{AccountId}/Points?{parameter.Key}={parameter.Value}";
                else
                    url += $"&{parameter.Key}={parameter.Value}";
            }

            if (url is null)
                throw new ArgumentException("At least one parameter must be provided.");

            return SendAsync<List<YulsnPoint>>(HttpMethod.Get, url, YulsnApiVersion.V2);
        }
    }
}

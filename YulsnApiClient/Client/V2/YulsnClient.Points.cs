using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            int? contactId = null,
            string sourceId = null,
            string type = null,
            DateTimeOffset? dateTimeFrom = null,
            DateTimeOffset? dateTimeTo = null,
            int? contactCompanyId = null,
            int? storeId = null,
            bool? includeCancelPoints = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (contactId.HasValue)
                parameters["contactId"] = contactId.Value.ToString();

            if (!string.IsNullOrEmpty(sourceId))
                parameters["sourceId"] = sourceId;

            if (!string.IsNullOrEmpty(type))
                parameters["type"] = type;

            if (dateTimeFrom.HasValue)
                parameters["datetimeFrom"] = dateTimeFrom.Value.ToString("O");

            if (dateTimeTo.HasValue)
                parameters["datetimeTo"] = dateTimeTo.Value.ToString("O");

            if (contactCompanyId.HasValue)
                parameters["contactCompanyId"] = contactCompanyId.Value.ToString();

            if (storeId.HasValue)
                parameters["storeId"] = storeId.Value.ToString();

            if (parameters.Count == 0)
                throw new ArgumentException("At least one parameter must be provided.");

            if (includeCancelPoints.HasValue)
                parameters["includeCancelPoints"] = includeCancelPoints.Value.ToString().ToLowerInvariant();

            string url = $"api/v2/{AccountId}/Points?" + string.Join("&", parameters.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));

            return SendAsync<List<YulsnPoint>>(HttpMethod.Get, url, YulsnApiVersion.V2);
        }
    }
}

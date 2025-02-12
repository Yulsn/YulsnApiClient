using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>
        /// Will get contact status for each segment.
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <param name="segmentIds">Array of segment ids</param>
        /// <param name="timeoutSec">Timeout in seconds. 30 is max</param>
        /// <returns></returns>
        public Task<YulsnSegmentContactStatus[]> GetSegmentContactStatusAsync(int contactId, IEnumerable<int> segmentIds, int? timeoutSec = null) =>
            SendAsync<YulsnSegmentContactStatus[]>(
                HttpMethod.Get,
                $"api/v2/{AccountId}/Segments/GetContactStatus/{contactId}/{string.Join(",", segmentIds)}?{nameof(timeoutSec)}={timeoutSec}",
                nameof(GetSegmentContactStatusAsync),
                YulsnApiVersion.V2);
    }
}
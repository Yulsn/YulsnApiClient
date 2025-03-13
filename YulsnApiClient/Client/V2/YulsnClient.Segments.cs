using System.Collections.Generic;
using System.Linq;
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
                $"api/v2/{AccountId}/Segments/GetContactStatus/{contactId}/{string.Join(",", segmentIds)}{(timeoutSec != null ? $"?{nameof(timeoutSec)}={timeoutSec}" : null)}",
                nameof(GetSegmentContactStatusAsync),
                YulsnApiVersion.V2);

        /// <summary>
        /// Will get contact status for a segment.
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <param name="segmentId">Id of the segment</param>
        /// <param name="timeoutSec">Timeout in seconds. 30 is max</param>
        /// <returns></returns>
        public async Task<YulsnSegmentContactStatus> GetSegmentContactStatusAsync(int contactId, int segmentId, int? timeoutSec = null)
        {
            return (await GetSegmentContactStatusAsync(contactId, new int[] { segmentId }, timeoutSec).ConfigureAwait(false)).FirstOrDefault();
        }

    }
}
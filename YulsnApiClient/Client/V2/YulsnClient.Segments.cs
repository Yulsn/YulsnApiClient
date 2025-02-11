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
        /// <param name="commaSepSegmentIds">Comma separated list of segment ids</param>
        /// <param name="timeoutSec">Timeout in seconds</param>
        /// <returns></returns>
        public Task<YulsnSegmentContactStatus[]> GetSegmentContactStatusAsync(int contactId, string commaSepSegmentIds, int? timeoutSec = null) =>
            SendAsync<YulsnSegmentContactStatus[]>(
                HttpMethod.Get,
                $"api/v2/{AccountId}/Segments/{contactId}/{commaSepSegmentIds}?{nameof(timeoutSec)}={timeoutSec}",
                nameof(GetSegmentContactStatusAsync),
                YulsnApiVersion.V2);
    }
}
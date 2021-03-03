using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YulsnApiClient.Client
{
    public partial class YulsnClient
    {
        /// <summary> Retrieve collection of country dial codes </summary>
        public Task<List<string>> GetCountryDialCodesAsync() =>
            SendAsync<List<string>>(HttpMethod.Get, $"api/internal/Countries/DialCodes");
    }
}

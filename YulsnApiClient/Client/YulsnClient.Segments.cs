using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Net.Http;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<int>> GetContactIds(int segmentId) =>
            sendAsync<List<int>>($"/api/v1/Segments/{segmentId}/GetContactIds");        
    }
}
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
        public Task<List<T>> GetObjectByView<T>(string viewName) =>
            SendAsync<List<T>>($"api/v1/Data/View/{viewName}");
    }
}
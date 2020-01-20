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
        public Task<List<T>> GetObjectByViewAsync<T>(string viewName) =>
            SendAsync<List<T>>($"api/v1/Data/View/{viewName}");

        public Task<List<T>> GetObjectByFunctionAsync<T>(string viewName, params string[] parameters) =>
            SendAsync<List<T>>(HttpMethod.Post, $"api/v1/Data/Function/{viewName}", parameters);

        public Task<List<T>> GetObjectByStoredProcedureAsync<T>(string procedureName, params string[] parameters) =>
             SendAsync<List<T>>(HttpMethod.Post, $"api/v1/Data/Procedure/{procedureName}", parameters);
    }
}
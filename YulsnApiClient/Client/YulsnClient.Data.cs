using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<T>> GetObjectByViewAsync<T>(string viewName) =>
            SendAsync<List<T>>($"api/v1/Data/View/{viewName}");

        public Task<List<T>> GetObjectByFunctionAsync<T>(string functionName, params string[] parameters) =>
            SendAsync<List<T>>(HttpMethod.Post, $"api/v1/Data/Function/{functionName}", parameters);

        public Task<List<T>> GetObjectByStoredProcedureAsync<T>(string procedureName, params string[] parameters) =>
             SendAsync<List<T>>(HttpMethod.Post, $"api/v1/Data/Procedure/{procedureName}", parameters);
    }
}
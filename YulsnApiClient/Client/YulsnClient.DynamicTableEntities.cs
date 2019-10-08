using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Linq;
using System.Net.Http;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>
        /// Will return all {tableName} entity ids
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>        
        public Task<List<int>> GetAllDynamicTableEntityIds(string tableName) => SendAsync<List<int>>($"api/v1/table/{tableName}");
        
        /// <summary>
        /// Will return {tableName} entity by id or null if not exists
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="id">Dynamic table entity id</param>
        public Task<YulsnDynamicTableEntity> GetDynamicTableEntityById(string tableName, int id) => SendAsync<YulsnDynamicTableEntity>($"api/v1/table/{tableName}/{id}");
        
        /// <summary>
        /// Will return {tableName} entity by secret or null if not exists
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="secret">Dynamic table entity secret</param>
        public Task<YulsnDynamicTableEntity> GetDynamicTableEntityBySecret(string tableName, string secret) => SendAsync<YulsnDynamicTableEntity>($"api/v1/table/{tableName}?secret={secret}");
        
        /// <summary>
        /// Will return {tableName} entity by external id or null if not exists
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="externalId">Dynamic table entity externalId</param>        
        public Task<YulsnDynamicTableEntity> GetDynamicTableEntityByExternalId(string tableName, string externalId) => SendAsync<YulsnDynamicTableEntity>($"api/v1/table/{tableName}?externalId={externalId}");

        /// <summary>
        /// Will return {tableName} entities with id higher than provided lastId up to limit (take)
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="lastId">Last {tableName} entity id</param>
        /// <param name="take">Number of dynamic table entities to return. Max is the default value</param>        
        public Task<List<YulsnDynamicTableEntity>> GetDynamicTableEntitiesByLastId(string tableName, int lastId, int take = 1000) => SendAsync<List<YulsnDynamicTableEntity>>($"api/v1/table/{tableName}?lastId={lastId}&take={take}");

        /// <summary>
        /// Will return all {tableName} entity externalIds
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>        
        public async Task<List<string>> GetAllDynamicTableEntityExternalIds(string tableName)
        {
            int lastId = 0;
            int take = 1000;

            List<string> retVal = new List<string>();
            List<YulsnDynamicTableEntity> result = null;

            do
            {
                result = await GetDynamicTableEntitiesByLastId(tableName, lastId, take);

                retVal.AddRange(result.Select(o => o.ExternalId));

                if (result.Count > 0)
                    lastId = result.Max(o => o.Id);

            } while (result.Count == take);

            return retVal;
        }

        /// <summary>
        /// Update {tableName} entity - only provided fields will be changed
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="id">Dynamic table entity id</param>
        /// <param name="updateFields">Dynamic table entity fields with new values that should be updated.</param>
        /// <returns></returns>
        public Task<YulsnDynamicTableEntity> UpdateDynamicTableEntity(string tableName, int id, Dictionary<string, object> updateFields)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/v1/table/{tableName}/{id}");

            request.Content = json(updateFields);

            return SendAsync<YulsnDynamicTableEntity>(request);
        }
    }
}
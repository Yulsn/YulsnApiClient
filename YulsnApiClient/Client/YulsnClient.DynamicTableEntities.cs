﻿using Newtonsoft.Json;
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
        public Task<List<int>> GetAllDynamicTableEntityIds(string tableName) =>
            SendAsync<List<int>>($"api/v1/table/{tableName}");

        /// <summary>
        /// Will return {tableName} entity by id or null if not exists
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="id">Dynamic table entity id</param>
        public Task<T> GetDynamicTableEntityById<T>(string tableName, int id) where T : YulsnReadDynamicTableEntity =>
            SendAsync<T>($"api/v1/table/{tableName}/{id}");

        /// <summary>
        /// Will return {tableName} entity by secret or null if not exists
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="secret">Dynamic table entity secret</param>
        public Task<T> GetDynamicTableEntityBySecret<T>(string tableName, string secret) where T : YulsnReadDynamicTableEntity =>
            SendAsync<T>($"api/v1/table/{tableName}?secret={secret}");

        /// <summary>
        /// Will return {tableName} entity by external id or null if not exists
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="externalId">Dynamic table entity externalId</param>        
        public Task<T> GetDynamicTableEntityByExternalId<T>(string tableName, int externalId) where T : YulsnReadDynamicTableEntity =>
            GetDynamicTableEntityByExternalId<T>(tableName, externalId.ToString());
        /// <summary>
        /// Will return {tableName} entity by external id or null if not exists
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="externalId">Dynamic table entity externalId</param>        
        public Task<T> GetDynamicTableEntityByExternalId<T>(string tableName, string externalId) where T : YulsnReadDynamicTableEntity =>
            SendAsync<T>($"api/v1/table/{tableName}?externalId={externalId}");

        /// <summary>
        /// Will return {tableName} entities with id higher than provided lastId up to limit (take)
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="lastId">Last {tableName} entity id</param>
        /// <param name="take">Number of dynamic table entities to return. Max is the default value</param>        
        public Task<List<T>> GetDynamicTableEntitiesByLastId<T>(string tableName, int lastId, int take = 1000) where T : YulsnReadDynamicTableEntity =>
            SendAsync<List<T>>($"api/v1/table/{tableName}?lastId={lastId}&take={take}");

        /// <summary>
        /// Will return all {tableName} entity externalIds
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>        
        public async Task<List<string>> GetAllDynamicTableEntityExternalIds(string tableName)
        {
            int lastId = 0;
            int take = 1000;

            List<string> retVal = new List<string>();
            List<YulsnReadDynamicTableEntity> result = null;

            do
            {
                result = await GetDynamicTableEntitiesByLastId<YulsnReadDynamicTableEntity>(tableName, lastId, take).ConfigureAwait(false);

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
        public Task<T> UpdateDynamicTableEntity<T>(string tableName, int id, Dictionary<string, object> updateFields) where T : YulsnReadDynamicTableEntity =>
            SendAsync<T>(new HttpMethod("PATCH"), $"api/v1/table/{tableName}/{id}", updateFields);

        /// <summary>
        /// Create {tableName} entity
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>        
        /// <param name="newEntity">Dynamic table entity to be inserted</param>
        /// <returns>The inserted entity added id, secret, created and last modified</returns>
        public Task<T> CreateDynamicTableEntity<T, R>(string tableName, R newEntity) where T : YulsnReadDynamicTableEntity where R : YulsnCreateDynamicTableEntity =>
            SendAsync<T>(HttpMethod.Post, $"api/v1/table/{tableName}", newEntity);


        /// <summary>
        /// Create {tableName} entities
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>        
        /// <param name="newEntities">Dynamic table entites to be inserted</param>
        /// <returns>The inserted entity added id, secret, created and last modified</returns>
        public Task CreateDynamicTableEntities<T>(string tableName, List<T> newEntities) where T : YulsnCreateDynamicTableEntity =>
            SendAsync<object>(HttpMethod.Post, $"api/v1/table/{tableName}/Bulk", newEntities);


        /// <summary>
        /// Will delete {tableName} entity by id
        /// </summary>
        /// <param name="tableName">Dynamic table name</param>
        /// <param name="id">Dynamic table entity id</param>
        public Task DeleteDynamicTableEntity(string tableName, int id) =>
            SendAsync<object>(HttpMethod.Delete, $"api/v1/table/{tableName}/{id}");
    }
}
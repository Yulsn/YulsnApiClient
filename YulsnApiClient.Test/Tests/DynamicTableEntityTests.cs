using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class DynamicTableEntityTests : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient;
        private readonly TestModel _model;

        public DynamicTableEntityTests(Setup setup)
        {
            _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
            _model = setup.Repository.Model;
        }

        [Fact]
        public async Task GetAllIds()
        {
            var list = await _yulsnClient.GetAllDynamicTableEntityIdsAsync(_model.ValidDynamicTableName);
            var list2 = await _yulsnClient.GetAllDynamicTableEntityExternalIdsAsync(_model.ValidDynamicTableName);

            Assert.NotNull(list);
            Assert.NotNull(list2);
            Assert.Equal(list.Count, list2.Count);
        }

        [Fact]
        public async Task GetByLastId()
        {
            int lastId = 0;

            var list = await _yulsnClient.GetDynamicTableEntitiesByLastIdAsync<YulsnReadDynamicTableEntity>(_model.ValidDynamicTableName, lastId);

            Assert.NotNull(list);
            Assert.True(list.Count > 0, "table with 0 enties not valid for test");
            Assert.True(list.Count < 1000, "table with more than 1000 enties not valid for test");
            Assert.Equal(list.Min(o => o.Id), list.First().Id);
            Assert.Equal(list.Max(o => o.Id), list.Last().Id);

            lastId = list.Min(o => o.Id);

            var list2 = await _yulsnClient.GetDynamicTableEntitiesByLastIdAsync<YulsnReadDynamicTableEntity>(_model.ValidDynamicTableName, lastId);

            Assert.NotNull(list2);
            Assert.Equal(list.Count - 1, list2.Count);

            lastId = list.Max(o => o.Id);

            var list3 = await _yulsnClient.GetDynamicTableEntitiesByLastIdAsync<YulsnReadDynamicTableEntity>(_model.ValidDynamicTableName, lastId);

            Assert.NotNull(list3);
            Assert.Empty(list3);
        }

        [Fact]
        public async Task GetEntity()
        {
            int id = _model.ValidDynamicEntityId;

            var entityById = await _yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(_model.ValidDynamicTableName, id);

            Assert.NotNull(entityById);
            Assert.Equal(id, entityById.Id);

            var entityBySecret = await _yulsnClient.GetDynamicTableEntityBySecretAsync<YulsnReadDynamicTableEntity>(_model.ValidDynamicTableName, entityById.Secret);

            Assert.NotNull(entityBySecret);
            Assert.Equal(entityById.Id, entityBySecret.Id);
            Assert.Equal(entityById.Secret, entityBySecret.Secret);
            Assert.Equal(entityById.Name, entityBySecret.Name);
            Assert.Equal(entityById.ExternalId, entityBySecret.ExternalId);
            Assert.Equal(entityById.Created, entityBySecret.Created);
            Assert.Equal(entityById.LastModified, entityBySecret.LastModified);

            var entityByExternalId = await _yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(_model.ValidDynamicTableName, entityBySecret.ExternalId);

            Assert.NotNull(entityByExternalId);
            Assert.Equal(entityBySecret.Id, entityByExternalId.Id);
            Assert.Equal(entityBySecret.Secret, entityByExternalId.Secret);
            Assert.Equal(entityBySecret.Name, entityByExternalId.Name);
            Assert.Equal(entityBySecret.ExternalId, entityByExternalId.ExternalId);
            Assert.Equal(entityBySecret.Created, entityByExternalId.Created);
            Assert.Equal(entityBySecret.LastModified, entityByExternalId.LastModified);
        }

        [Fact]
        public async Task UpdateEntity()
        {
            string newExternalId = Guid.NewGuid().ToString();

            var entity = await _yulsnClient.UpdateDynamicTableEntityAsync<YulsnReadDynamicTableEntity>(_model.ValidDynamicTableName, _model.ValidDynamicEntityId, new Dictionary<string, object> { { "ExternalId", newExternalId } });

            Assert.NotNull(entity);
            Assert.Equal(newExternalId, entity.ExternalId);
        }

        [Fact]
        public async Task CreateAndDeleteEntities()
        {
            string tableName = _model.ValidDynamicTableName;

            var newItem1 = new YulsnCreateDynamicTableEntity { Name = "Test item 1", ExternalId = Guid.NewGuid().ToString() };
            var newItem2 = new YulsnCreateDynamicTableEntity { Name = "Test item 2", ExternalId = Guid.NewGuid().ToString() };
            var newItem3 = new YulsnCreateDynamicTableEntity { Name = "Test item 3", ExternalId = Guid.NewGuid().ToString() };
            var newItem4 = new YulsnCreateDynamicTableEntity { Name = "Test item 4", ExternalId = Guid.NewGuid().ToString() };
            var newItem5 = new YulsnCreateDynamicTableEntity { Name = "Test item 5", ExternalId = Guid.NewGuid().ToString() };

            var createdEntity1 = await _yulsnClient.CreateDynamicTableEntityAsync<YulsnReadDynamicTableEntity, YulsnCreateDynamicTableEntity>(tableName, newItem1);

            testCreatedEntity(createdEntity1, newItem1);

            await _yulsnClient.CreateDynamicTableEntitiesAsync(tableName, [newItem2, newItem3, newItem4, newItem5]);

            var createdEntity2 = await _yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, newItem2.ExternalId);
            var createdEntity3 = await _yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, newItem3.ExternalId);
            var createdEntity4 = await _yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, newItem4.ExternalId);
            var createdEntity5 = await _yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, newItem5.ExternalId);

            testCreatedEntity(createdEntity2, newItem2);
            testCreatedEntity(createdEntity3, newItem3);
            testCreatedEntity(createdEntity4, newItem4);
            testCreatedEntity(createdEntity5, newItem5);

            await _yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity1.Id);
            await _yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity2.Id);
            await _yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity3.Id);
            await _yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity4.Id);
            await _yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity5.Id);

            createdEntity1 = await _yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity1.Id);
            createdEntity2 = await _yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity2.Id);
            createdEntity3 = await _yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity3.Id);
            createdEntity4 = await _yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity4.Id);
            createdEntity5 = await _yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity5.Id);

            Assert.Null(createdEntity1);
            Assert.Null(createdEntity2);
            Assert.Null(createdEntity3);
            Assert.Null(createdEntity4);
            Assert.Null(createdEntity5);

            void testCreatedEntity(YulsnReadDynamicTableEntity createdEntity, YulsnCreateDynamicTableEntity newItem)
            {
                Assert.NotNull(createdEntity);
                Assert.Equal(newItem.Name, createdEntity.Name);
                Assert.Equal(newItem.ExternalId, createdEntity.ExternalId);
                Assert.NotNull(createdEntity.Secret);
                Assert.NotEqual(0, createdEntity.Id);
                Assert.True(Math.Abs((createdEntity.Created.UtcDateTime - DateTime.UtcNow).TotalSeconds) < 10);
                Assert.True(Math.Abs((createdEntity.LastModified.UtcDateTime - DateTime.UtcNow).TotalSeconds) < 10);
            }
        }
    }
}

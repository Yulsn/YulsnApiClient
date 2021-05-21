using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models;

namespace YulsnApiClient.Test
{
    public class DynamicTableEntitiesTest : IClassFixture<Setup>
    {
        private readonly YulsnClient yulsnClient;
        private readonly string tableName;

        public DynamicTableEntitiesTest(Setup setup)
        {
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
            var config = setup.ServiceProvider.GetService<IConfiguration>();
            tableName = config["GetAllExternalIdTableName"];
        }

        [Fact]
        public async Task GetAllIds()
        {
            var list = await yulsnClient.GetAllDynamicTableEntityIdsAsync(tableName);
            var list2 = await yulsnClient.GetAllDynamicTableEntityExternalIdsAsync(tableName);

            Assert.NotNull(list);
            Assert.NotNull(list2);
            Assert.Equal(list.Count, list2.Count);
        }

        [Fact]
        public async Task GetByLastId()
        {
            int lastId = 0;

            var list = await yulsnClient.GetDynamicTableEntitiesByLastIdAsync<YulsnReadDynamicTableEntity>(tableName, lastId);

            Assert.NotNull(list);
            Assert.True(list.Count > 0, "table with 0 enties not valid for test");
            Assert.True(list.Count < 1000, "table with more than 1000 enties not valid for test");
            Assert.Equal(list.Min(o => o.Id), list.First().Id);
            Assert.Equal(list.Max(o => o.Id), list.Last().Id);

            lastId = list.Min(o => o.Id);

            var list2 = await yulsnClient.GetDynamicTableEntitiesByLastIdAsync<YulsnReadDynamicTableEntity>(tableName, lastId);

            Assert.NotNull(list2);
            Assert.Equal(list.Count - 1, list2.Count);

            lastId = list.Max(o => o.Id);

            var list3 = await yulsnClient.GetDynamicTableEntitiesByLastIdAsync<YulsnReadDynamicTableEntity>(tableName, lastId);

            Assert.NotNull(list3);
            Assert.Empty(list3);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetEntity(int id)
        {
            var entityById = await yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, id);

            Assert.NotNull(entityById);
            Assert.Equal(id, entityById.Id);

            var entityBySecret = await yulsnClient.GetDynamicTableEntityBySecretAsync<YulsnReadDynamicTableEntity>(tableName, entityById.Secret);

            Assert.NotNull(entityBySecret);
            Assert.Equal(entityById.Id, entityBySecret.Id);
            Assert.Equal(entityById.Secret, entityBySecret.Secret);
            Assert.Equal(entityById.Name, entityBySecret.Name);
            Assert.Equal(entityById.ExternalId, entityBySecret.ExternalId);
            Assert.Equal(entityById.Created, entityBySecret.Created);
            Assert.Equal(entityById.LastModified, entityBySecret.LastModified);

            var entityByExternalId = await yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, entityBySecret.ExternalId);

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

            var entity = await yulsnClient.UpdateDynamicTableEntityAsync<YulsnReadDynamicTableEntity>(tableName, 1, new Dictionary<string, object> { { "ExternalId", newExternalId } });

            Assert.NotNull(entity);
            Assert.Equal(newExternalId, entity.ExternalId);
        }


        [Fact]
        public async Task CreateAndDeleteEntities()
        {
            var newItem1 = new YulsnCreateDynamicTableEntity { Name = "Test item 1", ExternalId = "testitem1" };
            var newItem2 = new YulsnCreateDynamicTableEntity { Name = "Test item 2", ExternalId = "testitem2" };
            var newItem3 = new YulsnCreateDynamicTableEntity { Name = "Test item 3", ExternalId = "testitem3" };
            var newItem4 = new YulsnCreateDynamicTableEntity { Name = "Test item 4", ExternalId = "4444" };
            var newItem5 = new YulsnCreateDynamicTableEntity { Name = "Test item 5", ExternalId = "5555" };

            var createdEntity1 = await yulsnClient.CreateDynamicTableEntityAsync<YulsnReadDynamicTableEntity, YulsnCreateDynamicTableEntity>(tableName, newItem1);

            testCreatedEntity(createdEntity1, newItem1);

            await yulsnClient.CreateDynamicTableEntitiesAsync(tableName, new List<YulsnCreateDynamicTableEntity> { newItem2, newItem3, newItem4, newItem5 });

            var createdEntity2 = await yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, "testitem2");
            var createdEntity3 = await yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, "testitem3");
            var createdEntity4 = await yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, 4444);
            var createdEntity5 = await yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, 5555);

            testCreatedEntity(createdEntity2, newItem2);
            testCreatedEntity(createdEntity3, newItem3);
            testCreatedEntity(createdEntity4, newItem4);
            testCreatedEntity(createdEntity5, newItem5);

            await yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity1.Id);
            await yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity2.Id);
            await yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity3.Id);
            await yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity4.Id);
            await yulsnClient.DeleteDynamicTableEntityAsync(tableName, createdEntity5.Id);

            createdEntity1 = await yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity1.Id);
            createdEntity2 = await yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity2.Id);
            createdEntity3 = await yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity3.Id);
            createdEntity4 = await yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity4.Id);
            createdEntity5 = await yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, createdEntity5.Id);

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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using System.Linq;

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
            var list = await yulsnClient.GetAllDynamicTableEntityIds(tableName);
            var list2 = await yulsnClient.GetAllDynamicTableEntityExternalIds(tableName);

            Assert.NotNull(list);
            Assert.NotNull(list2);
            Assert.Equal(list.Count, list2.Count);
        }

        [Fact]
        public async Task GetByLastId()
        {
            int lastId = 0;

            var list = await yulsnClient.GetDynamicTableEntitiesByLastId(tableName, lastId);

            Assert.NotNull(list);
            Assert.True(list.Count > 0, "table with 0 enties not valid for test");
            Assert.True(list.Count < 1000, "table with more than 1000 enties not valid for test");
            Assert.Equal(list.Min(o => o.Id), list.First().Id);
            Assert.Equal(list.Max(o => o.Id), list.Last().Id);

            lastId = list.Min(o => o.Id);

            var list2 = await yulsnClient.GetDynamicTableEntitiesByLastId(tableName, lastId);

            Assert.NotNull(list2);
            Assert.Equal(list.Count - 1, list2.Count);

            lastId = list.Max(o => o.Id);

            var list3 = await yulsnClient.GetDynamicTableEntitiesByLastId(tableName, lastId);

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
            var entityById = await yulsnClient.GetDynamicTableEntityById(tableName, id);

            Assert.NotNull(entityById);
            Assert.Equal(id, entityById.Id);

            var entityBySecret = await yulsnClient.GetDynamicTableEntityBySecret(tableName, entityById.Secret);

            Assert.NotNull(entityBySecret);
            Assert.Equal(entityById.Id, entityBySecret.Id);
            Assert.Equal(entityById.Secret, entityBySecret.Secret);
            Assert.Equal(entityById.Name, entityBySecret.Name);
            Assert.Equal(entityById.ExternalId, entityBySecret.ExternalId);

            var entityByExternalId = await yulsnClient.GetDynamicTableEntityByExternalId(tableName, entityBySecret.ExternalId);

            Assert.NotNull(entityByExternalId);
            Assert.Equal(entityBySecret.Id, entityByExternalId.Id);
            Assert.Equal(entityBySecret.Secret, entityByExternalId.Secret);
            Assert.Equal(entityBySecret.Name, entityByExternalId.Name);
            Assert.Equal(entityBySecret.ExternalId, entityByExternalId.ExternalId);
        }

        [Fact]
        public async Task UpdateEntity()
        {
            string newExternalId = Guid.NewGuid().ToString();

            var entity = await yulsnClient.UpdateDynamicTableEntity(tableName, 1, new Dictionary<string, object> { { "ExternalId", newExternalId } });

            Assert.NotNull(entity);
            Assert.Equal(newExternalId, entity.ExternalId);
        }
    }
}

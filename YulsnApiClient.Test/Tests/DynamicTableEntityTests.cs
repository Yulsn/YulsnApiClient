using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class DynamicTableEntityTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

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

        [Fact]
        public async Task Get_Ok_WhenFoundById()
        {
            string tableName = _model.ValidDynamicTableName;
            int id = _model.ValidDynamicEntityId;

            IDictionary<string, object> response = await _yulsnClient.GetDynamicTableEntityByIdAsDictionaryAsync(tableName, id);

            ValidateDynamicEntity(response, expectedId: id, expectedDynamicField: _model.ValidDynamicEntityDynamicStringFieldValue);
        }

        [Fact]
        public async Task Get_NotFound_WhenInvalidId()
        {
            string tableName = _model.ValidDynamicTableName;
            int id = TestModel.InvalidId;

            YulsnReadDynamicTableEntity response = await _yulsnClient.GetDynamicTableEntityByIdAsync<YulsnReadDynamicTableEntity>(tableName, id);

            Assert.Equal(default, response);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundBySecret()
        {
            string tableName = _model.ValidDynamicTableName;
            string secret = _model.ValidDynamicEntitySecret;

            IDictionary<string, object> response = await _yulsnClient.GetDynamicTableEntityBySecretAsDictionaryAsync(tableName, secret);

            ValidateDynamicEntity(response, expectedSecret: secret, expectedDynamicField: _model.ValidDynamicEntityDynamicStringFieldValue);
        }

        [Fact]
        public async Task Get_NotFound_WhenInvalidSecret()
        {
            string tableName = _model.ValidDynamicTableName;
            string secret = TestModel.InvalidSecret;

            YulsnReadDynamicTableEntity response = await _yulsnClient.GetDynamicTableEntityBySecretAsync<YulsnReadDynamicTableEntity>(tableName, secret);

            Assert.Equal(default, response);
        }

        [Fact]
        public Task Get_Ok_WhenFoundByExternalId()
        {
            return Get_Ok_WhenFoundByExternalIdAsync(_model.ValidDynamicEntityExternalId, expectedDynamicField: _model.ValidDynamicEntityDynamicStringFieldValue);
        }

        [Fact]
        public async Task Get_NotFound_WhenInvalidExternalId()
        {
            string tableName = _model.ValidDynamicTableName;
            string externalId = TestModel.InvalidExternalId;

            YulsnReadDynamicTableEntity response = await _yulsnClient.GetDynamicTableEntityByExternalIdAsync<YulsnReadDynamicTableEntity>(tableName, externalId);

            Assert.Equal(default, response);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundByFields()
        {
            string tableName = _model.ValidDynamicTableName;

            List<Dictionary<string, object>> response = await _yulsnClient.SearchDynamicTableEntitiesAsDictionaryAsync(tableName, new YulsnSearchDynamicTableEntitiesDto
            {
                Fields =
                [
                    new YulsnSearchFieldDto
                    {
                        Field = nameof(YulsnReadDynamicTableEntity.ExternalId),
                        Operator = YulsnFieldFilterOperator.Equal,
                        Value = _model.ValidDynamicEntityExternalId
                    }
                ]
            });

            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.All(response, entity =>
            {
                ValidateDynamicEntity(entity,
                    expectedExternalId: _model.ValidDynamicEntityExternalId,
                    expectedDynamicField: _model.ValidDynamicEntityDynamicStringFieldValue);
            });
        }

        [Fact]
        public async Task Get_Ok_WhenFoundCountByFields()
        {
            string tableName = _model.ValidDynamicTableName;

            int response = await _yulsnClient.CountDynamicTableEntitiesAsync(tableName, new YulsnSearchDynamicTableEntitiesDto
            {
                Fields =
                [
                    new YulsnSearchFieldDto
                    {
                        Field = nameof(YulsnReadDynamicTableEntity.ExternalId),
                        Operator = YulsnFieldFilterOperator.Equal,
                        Value = _model.ValidDynamicEntityExternalId
                    }
                ]
            });

            Assert.Equal(1, response);
        }

        [Fact]
        public async Task PostPatchDelete_Success_WhenManage()
        {
            int entityId;

            string tableName = _model.ValidDynamicTableName;
            string externalId = Guid.NewGuid().ToString();

            // post an entity
            {
                Dictionary<string, object> requestContent = new()
                {
                    { nameof(YulsnCreateDynamicTableEntity.ExternalId), externalId },
                    { nameof(YulsnCreateDynamicTableEntity.Name), "Test Integration" },
                    { _model.ValidDynamicEntityDynamicStringFieldName, $"Test Integration {externalId}" },
                };

                IDictionary<string, object> response = await _yulsnClient.SendAsync<Dictionary<string, object>>(
                    request: new HttpRequestMessage(HttpMethod.Post, $"api/v1/table/{tableName}")
                    {
                        Content = _yulsnClient.JsonContent(requestContent)
                    },
                    apiVersion: YulsnApiVersion.V1);

                var result = ValidateDynamicEntity(response,
                    expectedExternalId: externalId,
                    expectedName: requestContent[nameof(YulsnReadDynamicTableEntity.Name)],
                    expectedDynamicField: requestContent[_model.ValidDynamicEntityDynamicStringFieldName]);

                entityId = result.id;
            }

            Dictionary<string, object> patchContent = new()
            {
                { nameof(YulsnCreateDynamicTableEntity.Name), "Patched Test Integration" },
                { _model.ValidDynamicEntityDynamicStringFieldName, $"Patched Test Integration {externalId}" },
            };

            // patch an entity
            {
                IDictionary<string, object> response = await _yulsnClient.SendAsync<Dictionary<string, object>>(
                    request: new HttpRequestMessage(HttpMethod.Patch, $"api/v1/table/{tableName}/{entityId}")
                    {
                        Content = _yulsnClient.JsonContent(patchContent)
                    },
                    apiVersion: YulsnApiVersion.V1);

                ValidateDynamicEntity(response,
                    expectedId: entityId,
                    expectedExternalId: externalId,
                    expectedName: patchContent[nameof(YulsnReadDynamicTableEntity.Name)],
                    expectedDynamicField: patchContent[_model.ValidDynamicEntityDynamicStringFieldName]);
            }

            // delete an entity
            {
                string deletedExternalId = await Delete_Ok_WhenFoundByIdAsync(entityId, expectedDynamicField: patchContent[_model.ValidDynamicEntityDynamicStringFieldName]);

                Assert.Equal(externalId, deletedExternalId);
            }
        }

        [Fact]
        public async Task Post_Conflict_WhenExternalIdExists()
        {
            string tableName = _model.ValidDynamicTableName;
            string externalId = _model.ValidDynamicEntityExternalId;

            try
            {
                await _yulsnClient.CreateDynamicTableEntityAsync<YulsnReadDynamicTableEntity, YulsnCreateDynamicTableEntity>(tableName, new YulsnCreateDynamicTableEntity
                {
                    ExternalId = externalId,
                    Name = "Test Integration"
                });

                Assert.Fail("Expected YulsnRequestException was not thrown.");
            }
            catch (YulsnRequestException ex)
            {
                Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
                Assert.Contains($"Cannot save duplicate value '{externalId}'!", ex.ErrorBody);
            }
        }

        [Fact]
        public async Task Patch_NotFound_WhenInvalidId()
        {
            string tableName = _model.ValidDynamicTableName;
            int entityId = TestModel.InvalidId;

            YulsnReadDynamicTableEntity response = await _yulsnClient.UpdateDynamicTableEntityAsync<YulsnReadDynamicTableEntity>(tableName, entityId, new Dictionary<string, object>
            {
                { nameof(YulsnCreateDynamicTableEntity.Name), "Invalid Patch Test" }
            });

            Assert.Equal(default, response);
        }

        [Fact]
        public async Task Patch_NotFound_WhenNoFields()
        {
            string tableName = _model.ValidDynamicTableName;
            int entityId = _model.ValidDynamicEntityId;

            YulsnReadDynamicTableEntity response = await _yulsnClient.UpdateDynamicTableEntityAsync<YulsnReadDynamicTableEntity>(tableName, entityId, []);

            Assert.Equal(default, response);
        }

        [Fact]
        public async Task Patch_BadRequest_WhenInvalidFields()
        {
            string tableName = _model.ValidDynamicTableName;
            int entityId = _model.ValidDynamicEntityId;

            try
            {
                await _yulsnClient.UpdateDynamicTableEntityAsync<YulsnReadDynamicTableEntity>(tableName, entityId, new Dictionary<string, object>
                {
                    { "Invalid_Field_Name_That_Should_Not_Exist", string.Empty }
                });

                Assert.Fail("Expected YulsnRequestException was not thrown.");
            }
            catch (YulsnRequestException ex)
            {
                Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
                Assert.Contains("There were no valid fields specified for patching!", ex.ErrorBody);
            }
        }

        [Fact]
        public async Task PostDelete_Success_WhenCreateBulk()
        {
            string tableName = _model.ValidDynamicTableName;
            string[] externalIds = [Guid.NewGuid().ToString(), Guid.NewGuid().ToString()];

            int index = 0;

            Dictionary<string, Dictionary<string, object>> entities = externalIds
                .ToDictionary(o => o, (o) =>
                {
                    index++;

                    return new Dictionary<string, object>()
                    {
                        { nameof(YulsnCreateDynamicTableEntity.ExternalId), o },
                        { nameof(YulsnCreateDynamicTableEntity.Name), $"Test Integration {index}/{externalIds.Length}" },
                        { _model.ValidDynamicEntityDynamicStringFieldName, $"DF Test Integration {index}/{externalIds.Length}" },
                    };
                });

            // create a bulk of entities
            {
                await _yulsnClient.CreateDynamicTableEntitiesAsync(tableName, [.. entities.Values]);
            }

            Dictionary<string, int> externalId_id_dict = [];

            // find all entities
            {
                foreach (var entity in entities)
                {
                    int entityId = await Get_Ok_WhenFoundByExternalIdAsync(
                        externalId: entity.Key,
                        expectedDynamicField: entity.Value[_model.ValidDynamicEntityDynamicStringFieldName]);

                    externalId_id_dict.Add(entity.Key, entityId);
                }

                Assert.Equal(externalId_id_dict.Count, externalIds.Length);
            }

            // delete all entities
            {
                List<string> deletedEntityExternalIds = [];

                foreach (var entity in entities)
                {
                    string externalId = await Delete_Ok_WhenFoundByIdAsync(
                        entityId: externalId_id_dict[entity.Key],
                        expectedDynamicField: entity.Value[_model.ValidDynamicEntityDynamicStringFieldName]);

                    deletedEntityExternalIds.Add(externalId);
                }

                Assert.Equal(deletedEntityExternalIds.Count, externalId_id_dict.Count);
                Assert.Empty(deletedEntityExternalIds.Except(externalId_id_dict.Keys));
            }
        }

        private (int id, string secret, string externalId, IDictionary<string, object> fields) ValidateDynamicEntity(IDictionary<string, object> fields,
            int? expectedId = null,
            string expectedSecret = null,
            string expectedExternalId = null,
            object expectedName = null,
            object expectedDynamicField = null)
        {
            Assert.True(fields.TryGetValueAsLong(nameof(YulsnReadDynamicTableEntity.Id), out long? _id));
            Assert.NotNull(_id);

            Assert.True(fields.TryGetValueAsString(nameof(YulsnReadDynamicTableEntity.Secret), out string _secret));
            Assert.NotNull(_secret);

            Assert.True(fields.TryGetValueAsString(nameof(YulsnReadDynamicTableEntity.ExternalId), out string _externalId));
            Assert.NotNull(_externalId);

            bool hasName = fields.TryGetValueAsString(nameof(YulsnReadDynamicTableEntity.Name), out string _name);

            Assert.Contains(_model.ValidDynamicEntityDynamicStringFieldName, fields.Keys);
            bool hasDynamicField = fields.TryGetValueAsString(_model.ValidDynamicEntityDynamicStringFieldName, out string _dynamicField);

            if (expectedId is not null)
            {
                Assert.Equal(expectedId, _id);
            }

            if (expectedSecret is not null)
            {
                Assert.Equal(expectedSecret, _secret);
            }

            if (expectedExternalId is not null)
            {
                Assert.Equal(expectedExternalId, _externalId);
            }

            if (expectedName is not null)
            {
                Assert.True(hasName);
                Assert.NotNull(_name);
                Assert.Equal(expectedName, _name);
            }

            if (expectedDynamicField is not null)
            {
                Assert.True(hasDynamicField);
                Assert.NotNull(_dynamicField);
                Assert.Equal(expectedDynamicField, _dynamicField);
            }

            return ((int)_id.Value, _secret, _externalId, fields);
        }

        private async Task<int> Get_Ok_WhenFoundByExternalIdAsync(string externalId, object expectedDynamicField = null)
        {
            string tableName = _model.ValidDynamicTableName;

            IDictionary<string, object> response = await _yulsnClient.GetDynamicTableEntityByExternalIdAsDictionaryAsync(tableName, externalId);

            var result = ValidateDynamicEntity(response, expectedExternalId: externalId, expectedDynamicField: expectedDynamicField);

            return result.id;
        }

        private async Task<string> Delete_Ok_WhenFoundByIdAsync(int entityId, object expectedDynamicField = null)
        {
            string tableName = _model.ValidDynamicTableName;

            IDictionary<string, object> response = await _yulsnClient.SendAsync<Dictionary<string, object>>(
                request: new HttpRequestMessage(HttpMethod.Delete, $"api/v1/table/{tableName}/{entityId}"),
                apiVersion: YulsnApiVersion.V1);

            var result = ValidateDynamicEntity(response, expectedId: entityId, expectedDynamicField: expectedDynamicField);

            return result.externalId;
        }
    }
}

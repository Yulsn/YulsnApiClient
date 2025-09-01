using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class OrderTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task Get_Ok_WhenFoundByExtOrderId()
        {
            string extOrderId = _model.ValidExtOrderId;

            List<Dictionary<string, object>> response = await _yulsnClient.GetOrdersAsDictionaryByExtOrderIdAsync(extOrderId);

            ValidateOrders(response, expectedExtOrderId: extOrderId);
        }

        [Fact]
        public async Task Get_Ok_WhenInvalidExtOrderId()
        {
            string extOrderId = TestModel.InvalidExtOrderId;

            List<Dictionary<string, object>> response = await _yulsnClient.GetOrdersAsDictionaryByExtOrderIdAsync(extOrderId);

            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundByContactId()
        {
            int contactId = _model.ValidOrderContactId;

            List<Dictionary<string, object>> response = await _yulsnClient.GetOrdersAsDictionaryByContactIdAsync(contactId);

            ValidateOrders(response, expectedContactId: contactId);
        }

        [Fact]
        public async Task Get_Ok_WhenInvalidContactId()
        {
            int contactId = TestModel.InvalidId;

            List<Dictionary<string, object>> response = await _yulsnClient.GetOrdersAsDictionaryByContactIdAsync(contactId);

            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundByContactIdAndType()
        {
            int contactId = _model.ValidOrderContactId;
            string orderType = _model.ValidOrderType;

            List<Dictionary<string, object>> response = await _yulsnClient.GetOrdersAsDictionaryByContactIdAsync(contactId, orderType);

            ValidateOrders(response, expectedContactId: contactId, expectedType: orderType);
        }

        [Fact]
        public async Task Get_Ok_WhenInvalidContactIdAndType()
        {
            int contactId = TestModel.InvalidId;
            string orderType = TestModel.InvalidOrderType;

            List<Dictionary<string, object>> response = await _yulsnClient.GetOrdersAsDictionaryByContactIdAsync(contactId, orderType);

            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundByContactIdAndTypeAndSkipOrderLines()
        {
            int contactId = _model.ValidOrderContactId;
            string orderType = _model.ValidOrderType;
            bool withOrderLines = false;

            List<Dictionary<string, object>> response = await _yulsnClient.GetOrdersAsDictionaryByContactIdAsync(contactId, orderType, withOrderLines);

            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.All(response, order =>
            {
                object orderlines = order.GetValueOrDefault(nameof(YulsnTestOrder.OrderLines));

                Assert.Null(orderlines);
            });
        }

        [Fact]
        public async Task Get_Ok_WhenFoundById()
        {
            int id = _model.ValidOrderId;

            Dictionary<string, object> response = await _yulsnClient.GetOrderAsDictionaryAsync(id);

            ValidateOrder(response,
                expectedId: id,
                expectedExtOrderId: _model.ValidExtOrderId,
                expectedType: _model.ValidOrderType,
                expectedDynamicField: _model.ValidOrderDynamicStringFieldValue,
                expectedOrderLineId: _model.ValidOrderLineId,
                onOrderLine: (fields, dynamicField) =>
                {
                    Assert.NotNull(dynamicField);
                    Assert.Equal(_model.ValidOrderLineDynamicStringFieldValue, dynamicField);
                });
        }

        [Fact]
        public async Task Get_NotFound_WhenInvalidId()
        {
            int id = TestModel.InvalidId;

            Dictionary<string, object> response = await _yulsnClient.GetOrderAsDictionaryAsync(id);

            Assert.Equal(default, response);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundValidOrderType()
        {
            string orderType = _model.ValidOrderType;

            List<string> response = await _yulsnClient.GetOrderTypesAsync();

            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Contains(orderType, response);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundByFields()
        {
            List<YulsnSearchFieldDto> filters =
            [
                new YulsnSearchFieldDto
                {
                    Field = nameof(YulsnTestOrder.ExtOrderId),
                    Operator = YulsnFieldFilterOperator.Equal,
                    Value = _model.ValidExtOrderId
                }
            ];

            List<Dictionary<string, object>> response = await _yulsnClient.SearchOrdersAsDictionaryAsync(filters);

            ValidateOrders(response, expectedExtOrderId: _model.ValidExtOrderId);
        }

        [Fact]
        public async Task PostPatchDelete_Success_WhenManage()
        {
            int orderId;
            int orderLineId;

            // post a new order
            {
                string extProductId = Guid.NewGuid().ToString();
                int orderLineQuantity = 2;
                decimal orderLinePrice = 20.00m;
                decimal orderLinePriceTotal = orderLineQuantity * orderLinePrice;

                Dictionary<string, object> requestContent = new()
                {
                    { nameof(YulsnTestOrder.Type), _model.ValidOrderType },
                    { nameof(YulsnTestOrder.ExtOrderId), Guid.NewGuid().ToString() },
                    { nameof(YulsnTestOrder.ContactId), _model.ValidOrderContactId },
                    { nameof(YulsnTestOrder.FirstName), "Test" },
                    { nameof(YulsnTestOrder.LastName), "Integration" },
                    { _model.ValidOrderDynamicStringFieldName, $"Test Integration {Guid.NewGuid():N}" },
                    { nameof(YulsnTestOrder.OrderLines), new List<Dictionary<string, object>>
                        {
                            new()
                            {
                                { nameof(YulsnTestOrderLine.ExtProductId), extProductId },
                                { nameof(YulsnTestOrderLine.Quantity), orderLineQuantity },
                                { nameof(YulsnTestOrderLine.Price), orderLinePrice },
                                { nameof(YulsnTestOrderLine.Description), "Integration Test" },
                                { _model.ValidOrderLineDynamicStringFieldName, $"Test Integration {Guid.NewGuid():N}" }
                            }
                        }
                    }
                };

                Dictionary<string, object> response = await _yulsnClient.CreateOrderAsDictionaryAsync(requestContent);

                bool validatedOrderLine = false;
                int _orderLineId = 0;
                Dictionary<string, object> orderLineContent = null;

                var result = ValidateOrder(response,
                    expectedType: _model.ValidOrderType,
                    expectedContactId: _model.ValidOrderContactId,
                    expectedExtOrderId: (requestContent[nameof(YulsnTestOrder.ExtOrderId)] as string),
                    expectedDynamicField: requestContent[_model.ValidOrderDynamicStringFieldName],
                    onOrderLine: (fields, dynamicField) =>
                    {
                        orderLineContent ??= fields;

                        Assert.NotNull(orderLineContent);

                        Assert.False(validatedOrderLine); // there must only be one order line

                        Assert.True(fields.TryGetValueAsLong(nameof(YulsnTestOrderLine.Id), out long? _id));
                        Assert.NotNull(_id);

                        Assert.True(fields.TryGetValueAsString(nameof(YulsnTestOrderLine.ExtProductId), out string? _extProductId));
                        Assert.NotNull(_extProductId);
                        Assert.Equal(_extProductId, extProductId);

                        Assert.True(fields.TryGetValueAsDouble(nameof(YulsnTestOrderLine.PriceTotal), out double? _priceTotal));
                        Assert.NotNull(_priceTotal);
                        Assert.Equal((decimal?)_priceTotal, orderLinePriceTotal);

                        Assert.Equal(orderLineContent[_model.ValidOrderLineDynamicStringFieldName], dynamicField);

                        _orderLineId = (int)_id.Value;

                        validatedOrderLine = true;
                    });

                orderId = result.id;
                orderLineId = _orderLineId;
            }

            // post new orderline
            {
                int orderLineQuantity = 4;
                decimal orderLinePrice = 22.00m;
                decimal orderLinePriceTotal = orderLineQuantity * orderLinePrice;

                Dictionary<string, object> requestContent = new()
                {
                    { nameof(YulsnTestOrderLine.ExtProductId), Guid.NewGuid().ToString() },
                    { nameof(YulsnTestOrderLine.Quantity), orderLineQuantity },
                    { nameof(YulsnTestOrderLine.Price), orderLinePrice },
                    { nameof(YulsnTestOrderLine.Description), "Integration Test 2" },
                    { _model.ValidOrderLineDynamicStringFieldName, $"Test Integration {Guid.NewGuid():N}" }
                };

                Dictionary<string, object> response = await _yulsnClient.CreateOrderLineAsDictionaryAsync(orderId, requestContent);

                ValidateOrderLine(response,
                    expectedOrderId: orderId,
                    onOrderLine: (fields, dynamicField) =>
                    {
                        string extProductId = requestContent[nameof(YulsnTestOrderLine.ExtProductId)] as string;

                        Assert.NotNull(extProductId);

                        Assert.True(fields.TryGetValueAsString(nameof(YulsnTestOrderLine.ExtProductId), out string? _extProductId));
                        Assert.NotNull(_extProductId);
                        Assert.Equal(_extProductId, extProductId);

                        Assert.True(fields.TryGetValueAsDouble(nameof(YulsnTestOrderLine.PriceTotal), out double? _priceTotal));
                        Assert.NotNull(_priceTotal);
                        Assert.Equal((decimal?)_priceTotal, orderLinePriceTotal);

                        Assert.Equal(requestContent[_model.ValidOrderLineDynamicStringFieldName], dynamicField);
                    });
            }

            // patch the order
            {
                Dictionary<string, object> requestContent = new()
                    {
                        { nameof(YulsnTestOrder.FirstName), "Patched Test" },
                        { _model.ValidOrderDynamicStringFieldName, $"Patched Test Integration {Guid.NewGuid():N}" }
                    };

                Dictionary<string, object> response = await _yulsnClient.UpdateOrderAsDictionaryAsync(orderId, requestContent);

                var result = ValidateOrder(response, expectedDynamicField: requestContent[_model.ValidOrderDynamicStringFieldName]);

                Assert.True(result.fields.TryGetValueAsString(nameof(YulsnTestOrder.FirstName), out string _firstName));
                Assert.NotNull(_firstName);
                Assert.Equal(_firstName, requestContent[nameof(YulsnTestOrder.FirstName)]);
            }

            // patch the order line
            {
                int newOrderLineQuantity = 5;
                decimal newOrderLinePrice = 33.00m;
                decimal newOrderLinePriceTotal = newOrderLineQuantity * newOrderLinePrice;

                Dictionary<string, object> requestContent = new()
                {
                    { nameof(YulsnTestOrderLine.Quantity), newOrderLineQuantity },
                    { nameof(YulsnTestOrderLine.Price), newOrderLinePrice },
                    { nameof(YulsnTestOrderLine.Description), "Patched Integration Test" },
                    { _model.ValidOrderLineDynamicStringFieldName, $"Patched Test Integration {Guid.NewGuid():N}" },
                };

                Dictionary<string, object> response = await _yulsnClient.UpdateOrderLineAsDictionaryAsync(orderId, orderLineId, requestContent);

                ValidateOrderLine(response,
                    expectedOrderId: orderId,
                    onOrderLine: (fields, dynamicField) =>
                    {
                        Assert.True(fields.TryGetValueAsDouble(nameof(YulsnTestOrderLine.PriceTotal), out double? _priceTotal));
                        Assert.NotNull(_priceTotal);
                        Assert.Equal((decimal?)_priceTotal, newOrderLinePriceTotal);

                        Assert.True(fields.TryGetValueAsString(nameof(YulsnTestOrderLine.Description), out string? _description));
                        Assert.NotNull(_description);
                        Assert.Equal(_description, requestContent[nameof(YulsnTestOrderLine.Description)]);

                        Assert.Equal(requestContent[_model.ValidOrderLineDynamicStringFieldName], dynamicField);
                    });
            }

            // delete the order line
            {
                Dictionary<string, object> response = await _yulsnClient.DeleteOrderLineAsDictionaryAsync(orderId, orderLineId);

                ValidateOrderLine(response,
                    expectedOrderId: orderId,
                    expectedOrderLineId: orderLineId);
            }

            // delete the order
            {
                Dictionary<string, object> response = await _yulsnClient.DeleteOrderAsDictionaryAsync(orderId);

                ValidateOrder(response,
                    expectedId: orderId);
            }

            // try to get the deleted order
            {
                Dictionary<string, object> response = await _yulsnClient.GetOrderAsDictionaryAsync(orderId);

                Assert.Equal(default, response);
            }
        }

        [Fact]
        public async Task Post_BadRequest_WhenNoLinesOnOrder()
        {
            Dictionary<string, object> requestContent = new()
            {
                { nameof(YulsnTestOrder.Type), _model.ValidOrderType },
                { nameof(YulsnTestOrder.FirstName), "Test" },
                { nameof(YulsnTestOrder.LastName), "Integration" },
            };

            try
            {
                await _yulsnClient.CreateOrderAsDictionaryAsync(requestContent);

                Assert.Fail("Expected YulsnRequestException was not thrown.");
            }
            catch (YulsnRequestException ex)
            {
                Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);

                ContentModel content = JsonConvert.DeserializeObject<ContentModel>(ex.ErrorBody);

                Assert.Equal("Invalid orderlines!", content.Message);
            }
        }

        [Fact]
        public async Task Post_BadRequest_WhenInvalidTypeByOrder()
        {
            Dictionary<string, object> requestContent = new()
            {
                { nameof(YulsnTestOrder.Type), TestModel.InvalidOrderType },
                { nameof(YulsnTestOrder.FirstName), "Test" },
                { nameof(YulsnTestOrder.LastName), "Integration" },
                { nameof(YulsnTestOrder.OrderLines), new List<Dictionary<string, object>>
                    {
                        new()
                        {
                            { nameof(YulsnTestOrderLine.Quantity), 1 },
                            { nameof(YulsnTestOrderLine.Price), 1.0M },
                            { nameof(YulsnTestOrderLine.Description), "Integration Test" },
                        }
                    }
                },
            };

            try
            {
                await _yulsnClient.CreateOrderAsDictionaryAsync(requestContent);

                Assert.Fail("Expected YulsnRequestException was not thrown.");
            }
            catch (YulsnRequestException ex)
            {
                Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);

                ContentModel content = JsonConvert.DeserializeObject<ContentModel>(ex.ErrorBody);

                Assert.Equal("Invalid order type!", content.Message);
            }
        }

        [Fact]
        public async Task Post_BadRequest_WhenInvalidOrderLineQuantity()
        {
            Dictionary<string, object> requestContent = new()
            {
                { nameof(YulsnTestOrder.Type), _model.ValidOrderType },
                { nameof(YulsnTestOrder.FirstName), "Test" },
                { nameof(YulsnTestOrder.LastName), "Integration" },
                { nameof(YulsnTestOrder.OrderLines), new List<Dictionary<string, object>>
                    {
                        new()
                        {

                            { nameof(YulsnTestOrderLine.Description), "Integration Test" },
                        }
                    }
                },
            };

            try
            {
                await _yulsnClient.CreateOrderAsDictionaryAsync(requestContent);

                Assert.Fail("Expected YulsnRequestException was not thrown.");
            }
            catch (YulsnRequestException ex)
            {
                Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);

                ContentModel content = JsonConvert.DeserializeObject<ContentModel>(ex.ErrorBody);

                Assert.Equal("Invalid orderlines!", content.Message);
            }
        }

        [Fact]
        public async Task Post_BadRequest_WhenInvalidOrderLinePrice()
        {
            Dictionary<string, object> requestContent = new()
            {
                { nameof(YulsnTestOrder.Type), _model.ValidOrderType },
                { nameof(YulsnTestOrder.FirstName), "Test" },
                { nameof(YulsnTestOrder.LastName), "Integration" },
                { nameof(YulsnTestOrder.OrderLines), new List<Dictionary<string, object>>
                    {
                        new()
                        {
                            { nameof(YulsnTestOrderLine.Quantity), 1 },
                            { nameof(YulsnTestOrderLine.Description), "Integration Test" },
                        }
                    }
                },
            };

            try
            {
                await _yulsnClient.CreateOrderAsDictionaryAsync(requestContent);

                Assert.Fail("Expected YulsnRequestException was not thrown.");
            }
            catch (YulsnRequestException ex)
            {
                Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);

                ContentModel content = JsonConvert.DeserializeObject<ContentModel>(ex.ErrorBody);

                Assert.Equal("Invalid orderlines!", content.Message);
            }
        }

        [Fact]
        public async Task Post_NotFound_WhenInvalidOrderLineByOrder()
        {
            int orderId = TestModel.InvalidId;

            Dictionary<string, object> requestContent = new()
            {
                { nameof(YulsnTestOrderLine.Quantity), 1 },
                { nameof(YulsnTestOrderLine.Price), 10.0M },
                { nameof(YulsnTestOrderLine.Description), "Integration Test" },
            };

            Dictionary<string, object> result = await _yulsnClient.CreateOrderLineAsDictionaryAsync(orderId, requestContent);

            Assert.Equal(default, result); // 404 returns default by the client implementation
        }

        [Fact]
        public async Task Post_BadRequest_WhenInvalidOrderLineQuantityByOrder()
        {
            int orderId = _model.ValidOrderId;

            Dictionary<string, object> requestContent = new()
            {
                { nameof(YulsnTestOrder.Type), _model.ValidOrderType },
                { nameof(YulsnTestOrder.FirstName), "Test" },
                { nameof(YulsnTestOrder.LastName), "Integration" },
                { nameof(YulsnTestOrder.OrderLines), new List<Dictionary<string, object>>
                    {
                        new()
                        {
                            { nameof(YulsnTestOrderLine.Price), 10.0M },
                            { nameof(YulsnTestOrderLine.Description), "Integration Test" },
                        }
                    }
                },
            };

            try
            {
                await _yulsnClient.CreateOrderLineAsDictionaryAsync(orderId, requestContent);

                Assert.Fail("Expected YulsnRequestException was not thrown.");
            }
            catch (YulsnRequestException ex)
            {
                Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);

                ContentModel content = JsonConvert.DeserializeObject<ContentModel>(ex.ErrorBody);

                Assert.Equal("Invalid order line!", content.Message);
            }
        }

        //[Fact]
        //public async Task Post_BadRequest_WhenInvalidOrderLinePriceByOrder()
        //{
        //    HttpResponseMessage response = await _client.PostAsync($"api/v1/Orders/{_model.ValidOrderId}/Lines", JsonContent.Create(new
        //    {
        //        Type = _model.ValidOrderType,
        //        FirstName = "Test",
        //        LastName = "Integration",
        //        OrderLines = new List<object>
        //        {
        //            new
        //            {
        //                Quantity = 1,
        //                Description = "Integration Test"
        //            }
        //        }
        //    }, options: _model.JsonSerializerOptions));

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}

        //[Fact]
        //public async Task Patch_NotFound_WhenOrderLineByInvalidOrder()
        //{
        //    int orderId = _model.InvalidId;
        //    int orderLineId = _model.ValidOrderIdAndSingleOrderLineId.Value;

        //    HttpResponseMessage response = await _client.PatchAsync($"api/v1/Orders/{orderId}/Lines/{orderLineId}", JsonContent.Create(new
        //    {
        //        Name = "Integration Test"
        //    }, options: _model.JsonSerializerOptions));

        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        //}

        //[Fact]
        //public async Task Patch_NotFound_WhenInvalidOrderLineByOrder()
        //{
        //    int orderId = _model.ValidOrderIdAndSingleOrderLineId.Key;
        //    int orderLineId = _model.InvalidId;

        //    HttpResponseMessage response = await _client.PatchAsync($"api/v1/Orders/{orderId}/Lines/{orderLineId}", JsonContent.Create(new
        //    {
        //        Name = "Integration Test"
        //    }, options: _model.JsonSerializerOptions));

        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        //}

        //[Fact]
        //public async Task Delete_NotFound_WhenOrderLineByInvalidOrder()
        //{
        //    HttpResponseMessage response = await _client.DeleteAsync($"api/v1/Orders/{_model.InvalidId}/Lines/{_model.InvalidId}");

        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        //}

        ///// <summary>Orderline cannot be deleted if it's the last one on the order</summary>
        //[Fact]
        //public async Task Delete_BadRequest_WhenValidOrderAndSingleOrderLine()
        //{
        //    int orderId = _model.ValidOrderIdAndSingleOrderLineId.Key;
        //    int orderLineId = _model.ValidOrderIdAndSingleOrderLineId.Value;

        //    HttpResponseMessage response = await _client.DeleteAsync($"api/v1/Orders/{orderId}/Lines/{orderLineId}");

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}

        //[Fact]
        //public async Task Delete_NotFound_WhenValidOrderAndInvalidOrderLine()
        //{
        //    HttpResponseMessage response = await _client.DeleteAsync($"api/v1/Orders/{_model.ValidOrderIdWithMultipleOrderLines}/Lines/{_model.InvalidId}");

        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        //}

        private void ValidateOrders(
            List<Dictionary<string, object>> response,
            string expectedExtOrderId = null,
            string expectedType = null,
            int? expectedContactId = null)
        {
            //List<Dictionary<string, object>?>? responseContent = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>?>>();

            Assert.NotNull(response);
            Assert.All(response, order => ValidateOrder(order,
                expectedExtOrderId: expectedExtOrderId,
                expectedType: expectedType,
                expectedContactId: expectedContactId));
        }

        //private async Task<(int id, string extOrderId, IDictionary<string, object> fields)> ValidateOrderAsync(HttpResponseMessage response,
        //   int? expectedId = null,
        //   string? expectedExtOrderId = null,
        //   string? expectedType = null,
        //   int? expectedContactId = null,
        //   object? expectedDynamicField = null,
        //   int? expectedOrderLineId = null,
        //   Action<Dictionary<string, object>, string?>? onOrderLine = null)
        //{
        //    Dictionary<string, object>? responseContent = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        //    return ValidateOrder(responseContent,
        //        expectedId: expectedId,
        //        expectedExtOrderId: expectedExtOrderId,
        //        expectedType: expectedType,
        //        expectedContactId: expectedContactId,
        //        expectedDynamicField: expectedDynamicField,
        //        expectedOrderLineId: expectedOrderLineId,
        //        onOrderLine: onOrderLine);
        //}

        //private async Task<Dictionary<string, object>> ValidateOrderLineAsync(HttpResponseMessage response,
        //    int? expectedOrderId = null,
        //    int? expectedOrderLineId = null,
        //    Action<Dictionary<string, object>, string?>? onOrderLine = null)
        //{
        //    Dictionary<string, object>? responseContent = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        //    Assert.NotNull(responseContent);

        //    IDictionary<string, object> fields = responseContent.ConvertJsonElements();

        //    return ValidateOrderLine(fields, expectedOrderId: expectedOrderId, expectedOrderLineId: expectedOrderLineId, onOrderLine: onOrderLine);
        //}

        private (int id, string extOrderId, IDictionary<string, object> fields) ValidateOrder(
            IDictionary<string, object> responseContent,
            int? expectedId = null,
            string expectedExtOrderId = null,
            string expectedType = null,
            int? expectedContactId = null,
            object expectedDynamicField = null,
            int? expectedOrderLineId = null,
            Action<Dictionary<string, object>, string> onOrderLine = null)
        {
            Assert.NotNull(responseContent);

            IDictionary<string, object> fields = responseContent.ConvertJsonElements();

            Assert.True(fields.TryGetValueAsLong(nameof(YulsnTestOrder.Id), out long? _id));
            Assert.NotNull(_id);

            Assert.True(fields.TryGetValueAsString(nameof(YulsnTestOrder.ExtOrderId), out string _extOrderId));
            Assert.NotNull(_extOrderId);

            Assert.True(fields.TryGetValueAsString(nameof(YulsnTestOrder.Type), out string _type));
            Assert.NotNull(_type);

            bool hasContactId = fields.TryGetValueAsLong(nameof(YulsnTestOrder.ContactId), out long? _contactId);

            Assert.Contains(_model.ValidOrderDynamicStringFieldName, fields.Keys);
            bool hasDynamicField = fields.TryGetValueAsString(_model.ValidOrderDynamicStringFieldName, out string _dynamicField);

            if (expectedId is not null)
            {
                Assert.Equal(expectedId, _id);
            }

            if (expectedExtOrderId is not null)
            {
                Assert.Equal(expectedExtOrderId, _extOrderId);
            }

            if (expectedType is not null)
            {
                Assert.Equal(expectedType, _type);
            }

            if (expectedContactId is not null)
            {
                Assert.True(hasContactId);
                Assert.NotNull(_contactId);
                Assert.Equal(expectedContactId, _contactId);
            }

            if (expectedDynamicField is not null)
            {
                Assert.True(hasDynamicField);
                Assert.NotNull(_dynamicField);
                Assert.Equal(expectedDynamicField, _dynamicField);
            }

            ValidateOrderLines(fields, expectedOrderId: (int)_id, expectedOrderLineId: expectedOrderLineId, onOrderLine: onOrderLine);

            return ((int)_id.Value, _extOrderId, fields);
        }

        private void ValidateOrderLines(IDictionary<string, object> orderFields, int? expectedOrderId = null, int? expectedOrderLineId = null, Action<Dictionary<string, object>, string> onOrderLine = null)
        {
            Assert.True(orderFields.TryGetValue(nameof(YulsnTestOrder.OrderLines), out object orderLinesObj));
            Assert.NotNull(orderLinesObj);

            List<object> orderLinesObjList = orderLinesObj as List<object>;
            Assert.NotNull(orderLinesObjList);
            Assert.NotEmpty(orderLinesObjList);

            List<Dictionary<string, object>> _orderLines = [.. orderLinesObjList
                .Select(orderLine => ValidateOrderLine(orderLine, expectedOrderId: expectedOrderId, expectedOrderLineId: expectedOrderLineId, onOrderLine: onOrderLine))];

            Assert.NotNull(_orderLines);
            Assert.NotEmpty(_orderLines);

            if (expectedOrderId is not null)
            {
                Assert.All(_orderLines, orderLine =>
                {
                    Assert.True(orderLine.TryGetValueAsLong(nameof(YulsnTestOrderLine.OrderId), out long? _orderId));
                    Assert.NotNull(_orderId);
                    Assert.Equal(expectedOrderId, _orderId);
                });
            }
        }

        private Dictionary<string, object> ValidateOrderLine(object obj, int? expectedOrderId = null, int? expectedOrderLineId = null, Action<Dictionary<string, object>, string> onOrderLine = null)
        {
            Dictionary<string, object> fields = obj as Dictionary<string, object>;

            Assert.NotNull(fields);
            Assert.NotEmpty(fields);

            Assert.True(fields.TryGetValueAsLong(nameof(YulsnTestOrderLine.Id), out long? _id));
            Assert.NotNull(_id);

            Assert.True(fields.TryGetValueAsLong(nameof(YulsnTestOrderLine.OrderId), out long? _orderId));
            Assert.NotNull(_orderId);

            Assert.True(fields.TryGetValueAsDouble(nameof(YulsnTestOrderLine.Quantity), out double? _quantity));
            Assert.NotNull(_quantity);
            Assert.NotEqual(0, _quantity);

            Assert.True(fields.TryGetValueAsDouble(nameof(YulsnTestOrderLine.Price), out double? _price));
            Assert.NotNull(_price);
            Assert.NotEqual(0, _price);

            Assert.Contains(_model.ValidOrderLineDynamicStringFieldName, fields.Keys);
            bool hasDynamicField = fields.TryGetValueAsString(_model.ValidOrderLineDynamicStringFieldName, out string _dynamicField);

            if (expectedOrderLineId is not null)
            {
                Assert.Equal(expectedOrderLineId, _id);
            }

            if (expectedOrderId is not null)
            {
                Assert.Equal(expectedOrderId, _orderId);
            }

            onOrderLine?.Invoke(fields, _dynamicField);

            return fields;
        }

        private class ContentModel
        {
            public required string Message { get; set; }
        }

        private class YulsnTestOrder : YulsnReadOrderDto<YulsnTestOrderLine> { }

        private class YulsnTestOrderLine : YulsnReadOrderLineDto
        {
            public decimal PriceTotal { get; set; }
        }
    }
}

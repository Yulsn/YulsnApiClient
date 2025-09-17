using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class PointTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task CreatePoint()
        {
            YulsnReadPointDto newPoint;

            {
                YulsnCreatePointDto model = new()
                {
                    Amount = 10,
                    ContactId = _model.ValidContactId,
                    Type = _model.ValidPointType,
                };

                newPoint = await _yulsnClient.CreatePointAsync(model);

                var point = await _yulsnClient.GetPointAsync(newPoint.Id);

                Assert.Equal(newPoint.Id, point.Id);
                Assert.Equal(newPoint.Amount, point.Amount);
                Assert.Equal(newPoint.Type, point.Type);
                Assert.Equal(newPoint.ContactId, point.ContactId);
                Assert.False(point.IsCanceller);
                Assert.Null(newPoint.CancellerPointId);
                Assert.Null(point.CancellerPointId);
            }

            {
                await _yulsnClient.CancelPointAsync(newPoint.Id);

                var cancelledPoint = await _yulsnClient.GetPointAsync(newPoint.Id);

                Assert.False(cancelledPoint.IsCanceller);
                Assert.NotNull(cancelledPoint.CancellerPointId);

                var cancellerPoint = await _yulsnClient.GetPointAsync(cancelledPoint.CancellerPointId.Value);

                Assert.True(cancellerPoint.IsCanceller);
                Assert.Equal(cancelledPoint.CancellerPointId.Value, cancellerPoint.Id);
                Assert.Equal(cancelledPoint.Amount, -cancellerPoint.Amount);
            }
        }

        [Fact]
        public async Task GetPoints()
        {
            DateTimeOffset startTime = DateTimeOffset.UtcNow;

            string sourceId_1 = "test_a";
            string sourceId_2 = "test_b";

            YulsnCreatePointDto p1 = new()
            {
                Amount = 10,
                ContactId = _model.ValidContactId,
                Type = _model.ValidPointType,
                SourceId = sourceId_1
            };

            YulsnCreatePointDto p2 = new()
            {
                Amount = 10,
                ContactId = _model.ValidContactId,
                Type = _model.ValidPointType,
                SourceId = sourceId_2
            };

            YulsnCreatePointDto p3 = new()
            {
                Amount = 10,
                ContactId = _model.ValidContactId,
                Type = _model.ValidPointType,
                SourceId = sourceId_1
            };

            var point1 = await _yulsnClient.CreatePointAsync(p1);
            var point2 = await _yulsnClient.CreatePointAsync(p2);
            var point3 = await _yulsnClient.CreatePointAsync(p3);

            var test1 = await _yulsnClient.GetPointsAsync(contactId: _model.ValidContactId);

            Assert.True(test1.Count >= 3);
            Assert.Contains(test1, o => o.Id == point1.Id);
            Assert.Contains(test1, o => o.Id == point2.Id);
            Assert.Contains(test1, o => o.Id == point3.Id);

            var test2 = await _yulsnClient.GetPointsAsync(contactId: _model.ValidContactId, type: _model.ValidPointType, sourceId: sourceId_1);
            Assert.Contains(test2, o => o.Id == point1.Id);
            Assert.Contains(test2, o => o.Id == point3.Id);
            Assert.DoesNotContain(test2, o => o.Id == point2.Id);

            var test3 = await _yulsnClient.GetPointsAsync(contactId: _model.ValidContactId, type: _model.ValidPointType, sourceId: sourceId_2);
            Assert.Contains(test3, o => o.Id == point2.Id);
            Assert.DoesNotContain(test3, o => o.Id == point1.Id);
            Assert.DoesNotContain(test3, o => o.Id == point3.Id);

            var test4 = await _yulsnClient.GetPointsAsync(contactId: _model.ValidContactId, type: _model.ValidPointType, sourceId: sourceId_1, dateTimeFrom: startTime.AddSeconds(-10));
            Assert.Contains(test4, o => o.Id == point1.Id);
            Assert.Contains(test4, o => o.Id == point3.Id);
            Assert.DoesNotContain(test4, o => o.Id == point2.Id);

            var test5 = await _yulsnClient.GetPointsAsync(contactId: _model.ValidContactId, type: null, dateTimeFrom: startTime.AddSeconds(-30));
            Assert.Contains(test5, o => o.Id == point1.Id);
            Assert.Contains(test5, o => o.Id == point2.Id);
            Assert.Contains(test5, o => o.Id == point3.Id);

            var test6 = await _yulsnClient.GetPointsAsync(contactId: _model.ValidContactId, type: _model.ValidPointType, dateTimeFrom: startTime.AddMinutes(-10), dateTimeTo: startTime.AddMinutes(-1));
            Assert.DoesNotContain(test6, o => o.Id == point1.Id);
            Assert.DoesNotContain(test6, o => o.Id == point3.Id);
            Assert.DoesNotContain(test6, o => o.Id == point2.Id);

            var test7 = await _yulsnClient.GetPointsAsync(contactId: _model.ValidContactId, type: _model.ValidPointType, dateTimeFrom: startTime.AddMinutes(1));
            Assert.DoesNotContain(test7, o => o.Id == point1.Id);
            Assert.DoesNotContain(test7, o => o.Id == point3.Id);
            Assert.DoesNotContain(test7, o => o.Id == point2.Id);

            await _yulsnClient.CancelPointAsync(point1.Id);
            await _yulsnClient.CancelPointAsync(point2.Id);
            await _yulsnClient.CancelPointAsync(point3.Id);
        }
    }
}

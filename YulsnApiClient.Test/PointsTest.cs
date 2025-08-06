using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Test
{
    public class PointsTest : IClassFixture<Setup>
    {
        private readonly IConfiguration config;
        private readonly YulsnClient yulsnClient;

        public PointsTest(Setup setup)
        {
            config = setup.ServiceProvider.GetService<IConfiguration>();
            yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        }

        [Fact]
        public async Task CreatePoint()
        {
            YulsnCreatePointDto model = new YulsnCreatePointDto
            {
                Amount = 10,
                ContactId = 1,
                Type = "test"
            };

            var point = await yulsnClient.CreatePointAsync(model);
            var pointGet = await yulsnClient.GetPointAsync(point.Id);

            Assert.Equal(point.Id, pointGet.Id);
            Assert.Equal(point.Amount, pointGet.Amount);
            Assert.Equal(point.Type, pointGet.Type);
            Assert.Equal(point.ContactId, pointGet.ContactId);
            Assert.True(!point.CancellerPointId.HasValue);
            Assert.True(!pointGet.CancellerPointId.HasValue);

            await yulsnClient.CancelPointAsync(point.Id);

            pointGet = await yulsnClient.GetPointAsync(point.Id);

            Assert.True(pointGet.CancellerPointId.HasValue);
        }

        [Fact]
        public async Task GetPoints()
        {
            DateTimeOffset startTime = DateTimeOffset.UtcNow;

            YulsnCreatePointDto p1 = new YulsnCreatePointDto
            {
                Amount = 10,
                ContactId = 1,
                Type = "test"
            };

            YulsnCreatePointDto p2 = new YulsnCreatePointDto
            {
                Amount = 10,
                ContactId = 1,
                Type = "test2"
            };

            YulsnCreatePointDto p3 = new YulsnCreatePointDto
            {
                Amount = 10,
                ContactId = 1,
                Type = "test"
            };

            var point1 = await yulsnClient.CreatePointAsync(p1);
            var point2 = await yulsnClient.CreatePointAsync(p2);
            var point3 = await yulsnClient.CreatePointAsync(p3);

            var test1 = await yulsnClient.GetPointsAsync(contactId: 1);

            Assert.True(test1.Count >= 3);
            Assert.Contains(test1, o => o.Id == point1.Id);
            Assert.Contains(test1, o => o.Id == point2.Id);
            Assert.Contains(test1, o => o.Id == point3.Id);

            var test2 = await yulsnClient.GetPointsAsync(contactId: 1, type: "test");
            Assert.Contains(test2, o => o.Id == point1.Id);
            Assert.Contains(test2, o => o.Id == point3.Id);
            Assert.DoesNotContain(test2, o => o.Id == point2.Id);

            var test3 = await yulsnClient.GetPointsAsync(contactId: 1, type: "test2");
            Assert.Contains(test3, o => o.Id == point2.Id);
            Assert.DoesNotContain(test3, o => o.Id == point1.Id);
            Assert.DoesNotContain(test3, o => o.Id == point3.Id);

            var test4 = await yulsnClient.GetPointsAsync(contactId: 1, type: "test", dateTimeFrom: startTime.AddSeconds(-10));
            Assert.Contains(test4, o => o.Id == point1.Id);
            Assert.Contains(test4, o => o.Id == point3.Id);
            Assert.DoesNotContain(test4, o => o.Id == point2.Id);

            var test5 = await yulsnClient.GetPointsAsync(contactId: 1, type: null, dateTimeFrom: startTime.AddSeconds(-30));
            Assert.Contains(test5, o => o.Id == point1.Id);
            Assert.Contains(test5, o => o.Id == point2.Id);
            Assert.Contains(test5, o => o.Id == point3.Id);

            var test6 = await yulsnClient.GetPointsAsync(contactId: 1, type: "test", dateTimeFrom: startTime.AddMinutes(-10), dateTimeTo: startTime.AddMinutes(-1));
            Assert.DoesNotContain(test6, o => o.Id == point1.Id);
            Assert.DoesNotContain(test6, o => o.Id == point3.Id);
            Assert.DoesNotContain(test6, o => o.Id == point2.Id);

            var test7 = await yulsnClient.GetPointsAsync(contactId: 1, type: "test", dateTimeFrom: startTime.AddMinutes(1));
            Assert.DoesNotContain(test7, o => o.Id == point1.Id);
            Assert.DoesNotContain(test7, o => o.Id == point3.Id);
            Assert.DoesNotContain(test7, o => o.Id == point2.Id);

            await yulsnClient.CancelPointAsync(point1.Id);
            await yulsnClient.CancelPointAsync(point2.Id);
            await yulsnClient.CancelPointAsync(point3.Id);
        }
    }
}

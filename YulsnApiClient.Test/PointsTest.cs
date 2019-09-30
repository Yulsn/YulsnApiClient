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
using YulsnApiClient.Models;

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
            YulsnCreatePointDto model = new YulsnCreatePointDto()
            {
                Amount = 10,
                ContactId = 1,
                Type = "test"
            };

            var point = await yulsnClient.CreatePoint(model);
            var pointGet = await yulsnClient.GetPoint(point.Id);


            Assert.True(point.Id == pointGet.Id);
            Assert.True(point.Amount == pointGet.Amount);
            Assert.True(point.Type == pointGet.Type);
            Assert.True(point.ContactId == pointGet.ContactId);
            Assert.True(!point.CancellerPointId.HasValue);
            Assert.True(!pointGet.CancellerPointId.HasValue);


            await yulsnClient.CancelPoint(point.Id);

            pointGet = await yulsnClient.GetPoint(point.Id);


            Assert.True(pointGet.CancellerPointId.HasValue);
        }
    }
}

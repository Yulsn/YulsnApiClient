using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class SegmentTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task Get_Ok_WhenCountContacts()
        {
            int segmentId = _model.ValidSegmentId;

            int response = await _yulsnClient.GetContactCountAsync(segmentId);

            Assert.True(response > 0);
        }

        [Fact]
        public async Task Get_Ok_WhenSegmentContactStatus()
        {
            int contactId = _model.ValidContactId;
            int[] segmentIds = [_model.ValidSegmentId];

            var response = await _yulsnClient.GetSegmentContactStatusAsync(contactId, segmentIds);

            Assert.NotNull(response);
            Assert.Equal(segmentIds.Length, response.Length);
        }
    }
}

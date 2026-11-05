using Xunit;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Test.Tests
{
    /// <summary>
    /// The bulk export type is sent as its number, so each value must match Loyaltii's
    /// <c>ExportType</c> enum. Pure test, no API call.
    /// </summary>
    public class ExportTypeContractTests
    {
        [Fact]
        public void Values_match_the_server_enum()
        {
            Assert.Equal(0, (int)ExportType.Contacts);
            Assert.Equal(21, (int)ExportType.VoucherAssignments);
            Assert.Equal(22, (int)ExportType.DeletedContacts);
        }
    }
}

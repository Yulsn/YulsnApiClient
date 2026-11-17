using Xunit;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Test.Tests
{
    /// <summary>
    /// The enums of the email campaign endpoints are sent as their numbers, so each value must match Loyaltii's
    /// <c>PublicApiModel.Enums</c>. Pure test, no API call.
    /// </summary>
    public class Email2CampaignContractTests
    {
        [Fact]
        public void FieldFilterOperator_values_match_the_server_enum()
        {
            Assert.Equal(1, (int)YulsnEmail2FieldFilterOperator.Equal);
            Assert.Equal(2, (int)YulsnEmail2FieldFilterOperator.NotEqual);
            Assert.Equal(7, (int)YulsnEmail2FieldFilterOperator.Null);
            Assert.Equal(8, (int)YulsnEmail2FieldFilterOperator.NotNull);
            Assert.Equal(9, (int)YulsnEmail2FieldFilterOperator.In);
            Assert.Equal(10, (int)YulsnEmail2FieldFilterOperator.NotIn);
            Assert.Equal(11, (int)YulsnEmail2FieldFilterOperator.Like);
        }

        [Fact]
        public void BlockInputType_values_match_the_server_enum()
        {
            Assert.Equal(1, (int)YulsnEmail2BlockInputType.Text);
            Assert.Equal(7, (int)YulsnEmail2BlockInputType.DataObject);
        }
    }
}

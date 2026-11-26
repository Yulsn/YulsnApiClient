using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Test.Tests
{
    /// <summary>
    /// The request bodies and responses of the external segment endpoints, as the client serializes and reads them.
    /// The server sends and reads enums as their names. Pure test, no API call.
    /// </summary>
    public class ExternalSegmentContractTests
    {
        private static JObject Serialize(object model) =>
            JObject.Parse(new YulsnClient(null, null).JsonContent(model).ReadAsStringAsync().Result);

        [Fact]
        public void Replace_sends_the_identifier_type_by_name()
        {
            JObject json = Serialize(new YulsnSegmentMembersReplace
            {
                IdentifierType = YulsnSegmentIdentifierType.SecretSha256,
                Contacts = new List<string> { "a", "b" }
            });

            Assert.Equal("SecretSha256", (string)json["IdentifierType"]);
            Assert.Equal(new[] { "a", "b" }, json["Contacts"].ToObject<string[]>());
        }

        [Fact]
        public void Patch_sends_add_and_remove()
        {
            JObject json = Serialize(new YulsnSegmentMembersPatch
            {
                IdentifierType = YulsnSegmentIdentifierType.ContactId,
                Add = new List<string> { "1" },
                Remove = new List<string> { "2" }
            });

            Assert.Equal("ContactId", (string)json["IdentifierType"]);
            Assert.Equal(new[] { "1" }, json["Add"].ToObject<string[]>());
            Assert.Equal(new[] { "2" }, json["Remove"].ToObject<string[]>());
        }

        [Fact]
        public void Refresh_reads_the_server_response()
        {
            YulsnSegmentRefresh refresh = JsonConvert.DeserializeObject<YulsnSegmentRefresh>(@"{
                ""id"": 7, ""segmentId"": 3, ""kind"": ""Replace"", ""status"": ""Completed"",
                ""startedAt"": ""2026-11-26T10:00:00+00:00"", ""completedAt"": ""2026-11-26T10:01:00+00:00"",
                ""rowsAdded"": 10, ""rowsRemoved"": 2, ""rowsUnmatched"": 1, ""error"": null }");

            Assert.Equal(7, refresh.Id);
            Assert.Equal(YulsnSegmentRefreshKind.Replace, refresh.Kind);
            Assert.Equal(YulsnSegmentRefreshStatus.Completed, refresh.Status);
            Assert.NotNull(refresh.CompletedAt);
            Assert.Equal(1, refresh.RowsUnmatched);
        }

        [Fact]
        public void Enum_values_match_the_server_enums()
        {
            Assert.Equal(1, (int)YulsnSegmentIdentifierType.ContactId);
            Assert.Equal(3, (int)YulsnSegmentIdentifierType.SecretSha256);
            Assert.Equal(2, (int)YulsnSegmentRefreshKind.Patch);
            Assert.Equal(3, (int)YulsnSegmentRefreshStatus.Failed);
        }
    }
}

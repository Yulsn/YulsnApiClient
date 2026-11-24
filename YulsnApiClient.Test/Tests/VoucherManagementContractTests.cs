using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Test.Tests
{
    /// <summary>
    /// The request bodies of the voucher, voucher group and media endpoints, as the client serializes them.
    /// Pure test, no API call.
    /// </summary>
    public class VoucherManagementContractTests
    {
        private class VoucherRequestWithFields : YulsnCreateVoucherRequest
        {
            public string Image { get; set; }
            public int Price { get; set; }
        }

        private static JObject Serialize(object model) =>
            JObject.Parse(new YulsnClient(null, null).JsonContent(model).ReadAsStringAsync().Result);

        [Fact]
        public void Voucher_dynamic_fields_are_sent_as_top_level_properties()
        {
            JObject json = Serialize(new YulsnUpdateVoucherRequest
            {
                Name = "Lip Mask",
                DynamicFields = new Dictionary<string, object>
                {
                    ["Image"] = "https://example.com/a.png",
                    ["Price"] = 100,
                    ["NavCouponReferenceNo"] = "C-1"
                }
            });

            Assert.Equal("Lip Mask", (string)json["Name"]);
            Assert.Equal("https://example.com/a.png", (string)json["Image"]);
            Assert.Equal(100, (int)json["Price"]);
            Assert.Equal("C-1", (string)json["NavCouponReferenceNo"]);
            Assert.Null(json["DynamicFields"]);
        }

        [Fact]
        public void Voucher_subclass_properties_are_sent()
        {
            JObject json = Serialize(new VoucherRequestWithFields { Name = "Lip Mask", Image = "https://example.com/a.png", Price = 100 });

            Assert.Equal("https://example.com/a.png", (string)json["Image"]);
            Assert.Equal(100, (int)json["Price"]);
        }

        [Fact]
        public void Voucher_group_requests_have_the_server_property_names()
        {
            JObject add = Serialize(new YulsnAddVoucherGroupVoucherRequest { VoucherId = 7, IsActive = true, AssignLimit = 3 });
            Assert.Equal(7, (int)add["VoucherId"]);
            Assert.True((bool)add["IsActive"]);
            Assert.Equal(3, (int)add["AssignLimit"]);
            Assert.Equal(JTokenType.Null, add["AssignWeight"].Type);

            JObject update = Serialize(new YulsnUpdateVoucherGroupVoucherRequest { IsActive = false, AssignWeight = 2 });
            Assert.False((bool)update["IsActive"]);
            Assert.Equal(2, (int)update["AssignWeight"]);
        }

        [Fact]
        public void Folder_type_is_sent_as_its_name()
        {
            JObject json = Serialize(new CreateYulsnFolder { Name = "Vouchers", Type = YulsnFolderType.Media });

            Assert.Equal("Media", (string)json["Type"]);
        }

        [Fact]
        public void Media_upload_sends_SkipOptimize()
        {
            JObject json = Serialize(new CreateYulsnMedia { Name = "a.png", FolderId = 1, ContentType = "image/png", Media = new byte[] { 1 } });

            Assert.False((bool)json["SkipOptimize"]);
        }
    }
}

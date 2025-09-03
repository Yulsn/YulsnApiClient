using System;
using System.Collections.Generic;

namespace YulsnApiClient.Test.Abstractions
{
    public enum TestEnvironment
    {
        Local_cta,
        Qa1
    }

    public class TestModel
    {
        public static int InvalidId => -1;
        public static string InvalidExtOrderId => "invalid-ext-order-id";
        public static string InvalidOrderType => "invalid-order-type";
        public static string InvalidSecret => "invalid-secret";
        public static string InvalidEmail => "invalid-email";
        public static string InvalidPhone => "invalid-phone";
        public static string InvalidExternalId => "invalid-externalId";

        // Contact (must be the same contact)
        public required int ValidContactId { get; set; }
        public required string ValidContactSecret { get; set; }
        public required string ValidContactEmail { get; set; }
        public required string ValidContactPhone { get; set; }
        public required string ValidContactDynamicStringFieldName { get; set; }
        public required string ValidContactDynamicStringFieldValue { get; set; }

        // Order (must be the same order)
        public required int ValidOrderId { get; set; }
        public required string ValidExtOrderId { get; set; }
        public required int ValidOrderContactId { get; set; }
        public required string ValidOrderType { get; set; }
        /// <summary>Id of a valid order with a single orderline id.</summary>
        public required KeyValuePair<int, int> ValidOrderIdAndSingleOrderLineId { get; set; }
        /// <summary>Id of a valid order with multiple orderlines.</summary>
        public required int ValidOrderIdWithMultipleOrderLines { get; set; }
        /// <summary>Belongs to the "ValidOrderId"</summary>
        public required int ValidOrderLineId { get; set; }
        public required string ValidOrderDynamicStringFieldName { get; set; }
        public required string ValidOrderDynamicStringFieldValue { get; set; }
        public required string ValidOrderLineDynamicStringFieldName { get; set; }
        public required string ValidOrderLineDynamicStringFieldValue { get; set; }

        // Dynamic entity (must be the same entity)
        public required string ValidDynamicTableName { get; set; }
        public required int ValidDynamicEntityId { get; set; }
        public required string ValidDynamicEntitySecret { get; set; }
        public required string ValidDynamicEntityExternalId { get; set; }
        public required int ValidDynamicEntityLastId { get; set; }
        public required string ValidDynamicEntityDynamicStringFieldName { get; set; }
        public required string ValidDynamicEntityDynamicStringFieldValue { get; set; }

        public required int ValidStoreId { get; set; }
        public required string ValidStoreNumber { get; set; }

        /// <summary>Must have contacts</summary>
        public required int ValidContactCompanyId { get; set; }
        public required string ValidContactCompanyPrimaryContactEmail { get; set; }
        /// <summary>A contact id that is not on linked to the valid contact company</summary>
        public required int UnlinkedContactCompanyContactId { get; set; }

        public required string ValidPointType { get; set; }

        /// <summary>For sending an email1 dispatch to the valid contact id</summary>
        public required int ValidEmailCampaignId { get; set; }

        /// <summary>For sending an email2 dispatch to the valid contact id</summary>
        public required string ValidEmail2MessageTriggerId { get; set; }

        /// <summary>There must always be at least 1 contact in this segment</summary>
        public required int ValidSegmentId { get; set; }

        /// <summary>For sending an push dispatch to the valid contact id</summary>
        public required string ValidPushMessageTriggerId { get; set; }

        /// <summary>For sending an sms dispatch to the valid contact id</summary>
        public required string ValidSmsMessageTriggerId { get; set; }
    }

    internal class TestRepository
    {
        public readonly TestModel Model;

        public TestRepository(TestEnvironment environment)
        {
            Model = environment switch
            {
                TestEnvironment.Local_cta => Local_cta,
                TestEnvironment.Qa1 => Qa1,
                _ => throw new Exception($"Invalid env: {environment}")
            };
        }

        public static TestModel Local_cta => new()
        {
            ValidContactId = 50499,
            ValidContactSecret = "CgkFS2XchZJO",
            ValidContactEmail = "test@integration.com",
            ValidContactPhone = "test-phone",
            ValidContactDynamicStringFieldName = "Metadata",
            ValidContactDynamicStringFieldValue = "d193a49759734751b9cba125aa6e020b",

            ValidOrderId = 391,
            ValidExtOrderId = "f060ce84-6d16-47a8-81da-467065e1e9a3",
            ValidOrderContactId = 50083,
            ValidOrderType = "test",
            ValidOrderIdAndSingleOrderLineId = new KeyValuePair<int, int>(402, 964),
            ValidOrderIdWithMultipleOrderLines = 440,
            ValidOrderLineId = 947,
            ValidOrderDynamicStringFieldName = "Description",
            ValidOrderDynamicStringFieldValue = "6d1647a8467065e1e81da9a3",
            ValidOrderLineDynamicStringFieldName = "Note",
            ValidOrderLineDynamicStringFieldValue = "5e1e816d1647a846706da9a3",

            ValidDynamicTableName = "Cars",
            ValidDynamicEntityId = 2,
            ValidDynamicEntitySecret = "62HICk7j",
            ValidDynamicEntityExternalId = "car2",
            ValidDynamicEntityLastId = 8,
            ValidDynamicEntityDynamicStringFieldName = "MetaData",
            ValidDynamicEntityDynamicStringFieldValue = "865khdMy8xSDr8SS",

            ValidStoreId = 2,
            ValidStoreNumber = "454",

            ValidContactCompanyId = 1,
            ValidContactCompanyPrimaryContactEmail = "cta+001@juhlsen.com",
            UnlinkedContactCompanyContactId = 8,

            ValidPointType = "testing",

            ValidEmailCampaignId = 262,

            ValidEmail2MessageTriggerId = "liquid-test",

            ValidSegmentId = 45,

            ValidPushMessageTriggerId = "trigger-demo",

            ValidSmsMessageTriggerId = "b-day",
        };

        public static TestModel Qa1 => new()
        {
            ValidContactId = 353034,
            ValidContactSecret = "mx7e9I38i1WG",
            ValidContactEmail = "test@integration.com",
            ValidContactPhone = "test-phone",
            ValidContactDynamicStringFieldName = "TempString",
            ValidContactDynamicStringFieldValue = "d193a4975973475120bb9cba125aa6e0",

            ValidOrderId = 1,
            ValidExtOrderId = "sample string 2",
            ValidOrderContactId = 1,
            ValidOrderType = "testsample",
            ValidOrderIdAndSingleOrderLineId = new KeyValuePair<int, int>(4, 7),
            ValidOrderIdWithMultipleOrderLines = 3,
            ValidOrderLineId = 1,
            ValidOrderDynamicStringFieldName = "DeliveryCompany",
            ValidOrderDynamicStringFieldValue = "move4u",
            ValidOrderLineDynamicStringFieldName = "DeliveryStatus",
            ValidOrderLineDynamicStringFieldValue = "finished",

            ValidDynamicTableName = "Cars",
            ValidDynamicEntityId = 4,
            ValidDynamicEntitySecret = "JuqwWvdJ",
            ValidDynamicEntityExternalId = "cw122",
            ValidDynamicEntityLastId = 9,
            ValidDynamicEntityDynamicStringFieldName = "Desc",
            ValidDynamicEntityDynamicStringFieldValue = "865kr8SShdMy8xSD",

            ValidStoreId = 1,
            ValidStoreNumber = "001",

            ValidContactCompanyId = 1,
            ValidContactCompanyPrimaryContactEmail = "",
            UnlinkedContactCompanyContactId = 8,

            ValidPointType = "test",

            ValidEmailCampaignId = 1,

            ValidEmail2MessageTriggerId = "test-email2",

            ValidSegmentId = 0,

            ValidPushMessageTriggerId = "test-push",

            ValidSmsMessageTriggerId = "test-sms",
        };
    }
}

using System;
using System.Collections.Generic;

namespace YulsnApiClient.Models
{
    public class YulsnCreateEmailDispatchDto
    {
        public int EmailCampaignId { get; set; }
        public int? ContactId { get; set; }
        public List<int> SegmentIds { get; set; }
        public int? OrderId { get; set; }
        public int? Order2Id { get; set; }
        public int? PointId { get; set; }
        public Dictionary<string, object> Dynamic { get; set; }
        public List<int> StoreIds { get; set; }
        public DateTimeOffset? DateTime { get; set; }
        public bool? IsTest { get; set; }
        public int? TakeRandom { get; set; }
        public YulsnEmailDispatchLogLevel LogLevel { get; set; }
    }

    public class YulsnReadEmailDispatchDto
    {
        public int Id { get; set; }
        public DateTimeOffset Planned { get; set; }
        public DateTime PlannedUtc { get; set; }
        public int EmailCampaignId { get; set; }
        public string EmailCampaignName { get; set; }
        public string Subject { get; set; }
        public string Preheader { get; set; }
        public YulsnEmailDispatchStatus Status { get; set; }
        public YulsnEmailDispatchType Type { get; set; }
        public int Sent { get; set; }
        public int Delivered { get; set; }
        public int Opens { get; set; }
        public int Clicks { get; set; }
        public decimal DeliveryRatio { get; set; }
        public decimal OpenedRatio { get; set; }
        public decimal ClickRatio { get; set; }

        // soon to be deprecated
        //public List<string> SegmentNames { get; set; }
    }

    public enum YulsnEmailDispatchLogLevel : int
    {
        None = 0,
        SmtpNoData = 1,
        SmtpWithData = 2
    }

    public enum YulsnEmailDispatchStatus : int
    {
        /// <summary>The dispatch has been created and not yet started</summary>
        Waiting = 100,
        /// <summary>Starting to add receivers to sql and smtp provider</summary>
        Preparing = 105,
        /// <summary>Dispatch is prepared and ready to start sending</summary>
        ReadyForStarting = 110,
        /// <summary>Attempt to start dispatch on Smtp.</summary>
        SmtpStarting = 112,
        /// <summary>The EmailDispatch has been sent to the smtp provider</summary>
        Sending = 115,
        /// <summary>All the mails are sent by the smtp provider</summary>
        Finished = 120,
        /// <summary>Manually cancelled</summary>
        Cancelled = 125,

        /// <summary>The error has not been added to {EmailDispatchStatus}</summary>
        EndedByUnknown = 500,
        /// <summary>The SengmentString was empty or there were no segment ids on the EmailDispatch</summary>
        EndedByInvalidSegment = 501,
        /// <summary>Html or Text must have a value</summary>
        EndedByHtmlAndText = 502,
        /// <summary>A new dispatch cannot have a version (on restart all versions should be deleted)</summary>
        EndedByVersionFound = 503,
        /// <summary>The contact (single receiver) was marked bounced at the time of dispatch start</summary>
        EndedByBounceMarkedContact = 504,
        /// <summary>The order is not found</summary>
        EndedByOrderNotFound = 505,
        /// <summary>The point is not found</summary>
        EndedByPointNotFound = 506,
        /// <summary>Could not find any or all stores</summary>
        EndedByStoresNotFound = 507,
        /// <summary>Error occured while attempting to save campaign values on dispatch</summary>
        EndedByUpdateByCampaign = 508,
        /// <summary>The smtp provider is missing or incomplete on the account</summary>
        EndedBySmtpProvider = 509,
        /// <summary>An api call to the smtp provider failed</summary>
        EndedBySmtpApi = 510,
        /// <summary>Error occured while checking template(s)</summary>
        EndedByTemplate = 511,
        /// <summary>Error occured while compiling template</summary>
        EndedByCompileTemplate = 512,
        /// <summary>There was not a single receiver with an email</summary>
        EndedByNoReceiver = 513,
        /// <summary>Failed by an Sql operation</summary>
        EndedBySqlException = 514,
        /// <summary>Prepared dispathc could not be started</summary>
        EndedBySmtpStartingFail = 515,
        /// <summary>An operation cancelled exception is thrown. Most likely by a WebJob Cancellation token.</summary>
        EndedByOperationCancelled = 516
    }

    public enum YulsnEmailDispatchType : int
    {
        Test = 1,
        Transactional,
        Newsletter
    }
}

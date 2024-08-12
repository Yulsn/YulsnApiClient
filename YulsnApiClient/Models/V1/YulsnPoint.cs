using System;

namespace YulsnApiClient.Models.V1
{
    public class YulsnReadPointDto
    {
        public int Id { get; set; }
        public int? ContactId { get; set; }
        public int? ContactCompanyId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Sum { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Description { get; set; }
        public string SourceId { get; set; }
        public string SourceDescription { get; set; }
        public int? StoreId { get; set; }
        public int? AdministratorId { get; set; }
        public decimal? BookValue { get; set; }
        public string SubType { get; set; }
        public decimal? PointCalculationBase { get; set; }
        public int? ConverterTransactionId { get; set; }
        public bool IsCanceller { get; set; }
        public int? CancellerPointId { get; set; }
    }

    public class YulsnCreatePointDto
    {
        public int ContactId { get; set; }
        public int? ContactCompanyId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string SourceId { get; set; }
        public string SourceDescription { get; set; }
        public int? StoreId { get; set; }
        public decimal? BookValue { get; set; }
        public string SubType { get; set; }
        public decimal? PointCalculationBase { get; set; }
    }

    public class YulsnReadPointSumDto
    {
        public int? ContactId { get; set; }
        public string Type { get; set; }
        public decimal Sum { get; set; }
        public DateTimeOffset LastModified { get; set; }
    }
}

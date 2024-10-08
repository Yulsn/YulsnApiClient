﻿namespace YulsnApiClient.Models.V1
{
    public class YulsnSearchFieldDto
    {
        public string Field { get; set; }
        public YulsnFieldFilterOperator Operator { get; set; }
        public string Value { get; set; }
        public string[] Values { get; set; }
    }

    public enum YulsnFieldFilterOperator
    {
        Equal = 1,
        NotEqual = 2,
        MoreThan = 3,
        MoreThanOrEqual = 4,
        LessThan = 5,
        LessThanOrEqual = 6,
        Null = 7,
        NotNull = 8,
        In = 9
    }
}

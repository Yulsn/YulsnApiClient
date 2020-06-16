using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models
{
    public class YulsnSearchFileldDto
    {
        public string Field { get; set; }
        public YulsnFieldFilterOperator Operator { get; set; }
        public string Value { get; set; }
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

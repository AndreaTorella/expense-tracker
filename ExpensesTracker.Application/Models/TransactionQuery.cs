using ExpensesTracker.Application.Enums;
using ExpensesTracker.Domain.Enums;

namespace ExpensesTracker.Application.Models
{
    public class TransactionQuery
    {
        public string? Search { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public TransactionType? TransactionType { get; set; }

        public int? CategoryId { get; set; }

        public int? PaymentMethodId { get; set; }

        public TransactionSortBy TransactionSortBy { get; set; }
            = TransactionSortBy.Date;

        public SortDirection SortDirection { get; set; }
            = SortDirection.Desc;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}

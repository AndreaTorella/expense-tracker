namespace ExpensesTracker.Models
{
    public class ExpenseFilterDto
    {
        public string? Search { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? CategoryId { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}

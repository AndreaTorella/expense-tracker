namespace ExpensesTracker.Models
{
    public class ExpenseListDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public CategoryName CategoryName { get; set; }
        public PaymentMethodName PaymentMethodName { get; set; }
    }
}

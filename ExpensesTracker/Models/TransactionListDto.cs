namespace ExpensesTracker.Models
{
    public class TransactionListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public int CategoryId { get; set; }
        public CategoryName CategoryName { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethodName PaymentMethodName { get; set; }
    }
}

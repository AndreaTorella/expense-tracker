namespace ExpensesTracker.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;
    }
}

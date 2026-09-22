using ExpensesTracker.Domain.Enums;

namespace ExpensesTracker.Application.Models
{
    public class CreateTransactionQuery
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public int CategoryId { get; set; }
        public int PaymentMethodId { get; set; }
    }
}

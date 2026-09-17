using ExpensesTracker.Domain.Enums;

namespace ExpensesTracker.Domain.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public PaymentMethodName Name { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

namespace ExpensesTracker.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public PaymentMethodName Name { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

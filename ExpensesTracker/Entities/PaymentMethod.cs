namespace ExpensesTracker.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public PaymentMethodName Name { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
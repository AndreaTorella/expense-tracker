namespace ExpensesTracker.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public CategoryName Name { get; set; }
        public TransactionType TransactionType { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

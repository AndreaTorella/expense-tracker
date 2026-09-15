namespace ExpensesTracker.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionType TransactionType { get; set; }

        public int HouseholdId { get; set; }
        public Household Household { get; set; } = null!;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public string CreatedByUserId { get; set; } = string.Empty;
        public ApplicationUser CreatedByUser { get; set; } = null!;
    }
}

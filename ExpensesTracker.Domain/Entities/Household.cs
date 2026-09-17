namespace ExpensesTracker.Domain.Entities
{
    public class Household
    {
        public int Id { get; set; }
        public string FamilyName { get; set; } = string.Empty;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

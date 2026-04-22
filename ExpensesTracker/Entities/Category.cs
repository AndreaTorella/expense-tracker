namespace ExpensesTracker.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public CategoryName Name { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
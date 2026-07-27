namespace ExpensesTracker.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public CategoryName Name { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}

namespace ExpensesTracker.Application.Models
{
    public class CategoryTotal
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Total { get; set; }
    }
}

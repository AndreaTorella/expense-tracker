namespace ExpensesTracker.Models.Common
{
    public class CategoryTotal
    {
        public int CategoryId { get; set; }

        public CategoryName CategoryName { get; set; } = CategoryName.Various;

        public decimal Total { get; set; }
    }
}

namespace ExpensesTracker.Models.Dashboard
{
    public class CategoryTotalDto
    {
        public CategoryName CategoryName { get; set; } = CategoryName.Various;

        public decimal Total { get; set; }
    }
}

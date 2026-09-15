namespace ExpensesTracker.Models.Dashboard
{
    public class CategoryTotalDto
    {
        public string CategoryName { get; set; } = string.Empty;

        public decimal Total { get; set; }
    }
}

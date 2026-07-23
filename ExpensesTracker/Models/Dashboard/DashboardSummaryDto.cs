namespace ExpensesTracker.Models.Dashboard
{
    public class DashboardSummaryDto
    {
        public decimal CurrentMonthTotal { get; set; }

        public decimal PreviousMonthTotal { get; set; }

        public decimal? DifferencePercentage { get; set; }

        public IEnumerable<CategoryTotalDto> CategoryTotals { get; set; } = [];

        public IEnumerable<MonthlyTotalDto> MonthlyTotals { get; set; } = [];
    }
}

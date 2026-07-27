namespace ExpensesTracker.Models.Dashboard
{
    public class DashboardSummaryDto
    {
        public decimal CurrentMonthTotalExpenses { get; set; }

        public decimal CurrentMonthTotalIncomes { get; set; }

        public decimal CurrentMonthBalance { get; set; }

        public decimal PreviousMonthTotalExpenses { get; set; }

        public decimal? DifferenceExpensePercentage { get; set; }

        public decimal? DifferenceIncomePercentage { get; set; }

        public IEnumerable<CategoryTotalDto> CategoryTotals { get; set; } = [];

        public IEnumerable<MonthlyTotalDto> MonthlyTotals { get; set; } = [];
    }
}

namespace ExpensesTracker.Application.Models
{
    public class DashboardSummaryResult
    {
        public decimal CurrentMonthTotalExpenses { get; set; }
        public decimal CurrentMonthTotalIncomes { get; set; }
        public decimal CurrentMonthBalance { get; set; }
        public decimal PreviousMonthTotalExpenses { get; set; }


        public decimal? DifferenceExpensePercentage { get; set; }
        public decimal? DifferenceIncomePercentage { get; set; }


        public IEnumerable<CategoryTotal> CategoryTotals { get; set; } = [];
        public IEnumerable<MonthlyTotal> MonthlyTotals { get; set; } = [];
    }
}

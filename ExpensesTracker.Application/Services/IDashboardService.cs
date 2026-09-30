using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryResult> GetSummaryAsync(DashboardQuery filters);
    }
}

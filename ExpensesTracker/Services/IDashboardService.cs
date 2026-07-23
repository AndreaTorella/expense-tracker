using ExpensesTracker.Models.Dashboard;

namespace ExpensesTracker.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(DashboardFilterDto filters);
    }
}

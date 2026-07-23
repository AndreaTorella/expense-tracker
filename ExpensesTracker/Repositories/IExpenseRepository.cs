using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Models.Common;

namespace ExpensesTracker.Repositories
{
    public interface IExpenseRepository
    {
        Task<PagedResult<Expense>> GetExpensesAsync(ExpenseFilterDto filters);
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task AddExpenseAsync(Expense expense);
        void DeleteExpenseAsync(Expense expense);
        Task SaveChangesAsync();

        Task<decimal> GetTotalAsync(DateTime fromDate, DateTime toDate);

        Task<IEnumerable<CategoryTotal>> GetTotalsByCategoryAsync(DateTime fromDate, DateTime toDate);

        Task<IEnumerable<MonthlyTotal>> GetMonthlyTotalsAsync(DateTime fromDate, DateTime toDate);
    }
}
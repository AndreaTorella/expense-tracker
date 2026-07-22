using ExpensesTracker.Entities;
using ExpensesTracker.Models;

namespace ExpensesTracker.Repositories
{
    public interface IExpenseRepository
    {
        Task<PagedResult<Expense>> GetExpensesAsync(ExpenseFilterDto filters);
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task AddExpenseAsync(Expense expense);
        void DeleteExpenseAsync(Expense expense);
        Task SaveChangesAsync();
    }
}
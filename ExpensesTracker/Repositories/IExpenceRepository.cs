using ExpensesTracker.Entities;

namespace ExpensesTracker.Repositories
{
    public interface IExpenceRepository
    {
        Task<IEnumerable<Expense>> GetAllExpensesAsync();
        Task<Expense> GetExpenseByIdAsync(int id);
        Task AddExpenseAsync(Expense expense);
        Task DeleteExpenseByIdAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
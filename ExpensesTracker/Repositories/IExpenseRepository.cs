using ExpensesTracker.Entities;

namespace ExpensesTracker.Repositories
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetAllExpensesAsync();
        Task<Expense?> GetExpenseByIdAsync(int id, bool includeCategory, bool includePaymentMethod);
        Task AddExpenseAsync(Expense expense);
        void DeleteExpenseAsync(Expense expense);
        Task SaveChangesAsync();
    }
}
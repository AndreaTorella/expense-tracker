using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Models.Common;

namespace ExpensesTracker.Repositories
{
    public interface ITransactionRepository
    {
        Task<PagedResult<Transaction>> GetTransactionAsync(TransactionFilterDto filters, int householdId);
        Task<Transaction?> GetTransactionByIdAsync(int id, int householdId);
        Task AddTransactionAsync(Transaction transaction);
        void DeleteTransactionAsync(Transaction transaction);
        Task SaveChangesAsync();

        Task<decimal> GetTotalAsync(DateTime fromDate, DateTime toDate, TransactionType transactionType, int householdId);

        Task<IEnumerable<CategoryTotal>> GetTotalsByCategoryAsync(
            DateTime fromDate,
            DateTime toDate,
            TransactionType transactionType,
            int householdId);

        Task<IEnumerable<MonthlyTotal>> GetMonthlyTotalsAsync(
            DateTime fromDate,
            DateTime toDate,
            TransactionType transactionType,
            int householdId);
    }
}

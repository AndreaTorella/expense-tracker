using ExpensesTracker.Application.Common;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Domain.Entities;
using ExpensesTracker.Domain.Enums;

namespace ExpensesTracker.Repositories
{
    public interface ITransactionRepository
    {
        Task<PagedResult<Transaction>> GetTransactionAsync(TransactionQuery filters, int householdId);
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

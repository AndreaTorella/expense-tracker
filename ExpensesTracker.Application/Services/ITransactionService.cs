using ExpensesTracker.Application.Common;
using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface ITransactionService
    {
        Task<PagedResult<TransactionResult>> GetAllTransactionsAsync(TransactionQuery filters);
        Task<TransactionResult?> GetTransactionByIdAsync(int id);
        Task<TransactionResult> AddTransactionAsync(CreateTransactionCommand transactionDto);
        Task<TransactionResult?> UpdateTransactionAsync(int id, UpdateTransactionCommand updateTransactionDto);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}

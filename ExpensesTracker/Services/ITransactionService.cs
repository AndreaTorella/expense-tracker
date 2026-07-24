using ExpensesTracker.Models;

namespace ExpensesTracker.Services
{
    public interface ITransactionService
    {
        Task<PagedResultDto<TransactionListDto>> GetAllTransactionsAsync(TransactionFilterDto filters);
        Task<TransactionListDto?> GetTransactionByIdAsync(int id);
        Task<TransactionListDto> AddTransactionAsync(CreateTransactionDto transactionDto);
        Task<bool> DeleteTransactionAsync(int transactionId);
        Task<TransactionListDto?> UpdateTransactionAsync(int id, UpdateTransactionDto updateTransactionDto);
    }
}

using ExpensesTracker.Models;

namespace ExpensesTracker.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseListDto>> GetAllExpensesAsync();
        Task<ExpenseListDto?> GetExpenseByIdAsync(int id, bool includeCategory, bool includePaymentMethod);
        Task<ExpenseListDto> AddExpenseAsync(CreateExpenseDto expenseDto);
        Task<bool> DeleteExpenseAsync(int expenseId);
    }
}
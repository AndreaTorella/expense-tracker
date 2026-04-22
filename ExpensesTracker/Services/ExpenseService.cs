using AutoMapper;
using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Repositories;

namespace ExpensesTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository expenseRepository;
        private readonly IMapper mapper;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            IMapper mapper)
        {
            this.expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ExpenseListDto>> GetAllExpensesAsync()
        {
            var expenseEntities = await expenseRepository.GetAllExpensesAsync();
            return mapper.Map<IEnumerable<ExpenseListDto>>(expenseEntities);
        }

        public async Task<ExpenseListDto?> GetExpenseByIdAsync(int id, bool includeCategory, bool includePaymentMethod)
        {
            var expenseEntity = await expenseRepository.GetExpenseByIdAsync(id, includeCategory, includePaymentMethod);

            if (expenseEntity == null)
            {
                return null;
            }

            return mapper.Map<ExpenseListDto>(expenseEntity);
        }

        public async Task<ExpenseListDto> AddExpenseAsync(CreateExpenseDto expenseDto)
        {
            if (expenseDto == null)
            {
                throw new ArgumentNullException(nameof(expenseDto));
            }

            var expenseEntity = mapper.Map<Expense>(expenseDto);
            await expenseRepository.AddExpenseAsync(expenseEntity);
            await expenseRepository.SaveChangesAsync();

            return mapper.Map<ExpenseListDto>(expenseEntity);
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            var expenseEntityToDelete = await expenseRepository.GetExpenseByIdAsync(expenseId, false, false);

            if (expenseEntityToDelete == null)
            {
                return false;
            }

            expenseRepository.DeleteExpenseAsync(expenseEntityToDelete);
            await expenseRepository.SaveChangesAsync();
            return true;
        }
    }
}
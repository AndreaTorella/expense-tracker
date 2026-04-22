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
            var expenseEntities = await this.expenseRepository.GetAllExpensesAsync();
            return this.mapper.Map<IEnumerable<ExpenseListDto>>(expenseEntities);
        }

        public async Task<ExpenseListDto?> GetExpenseByIdAsync(int id, bool includeCategory, bool includePaymentMethod)
        {
            var expenseEntity = await this.expenseRepository.GetExpenseByIdAsync(id, includeCategory, includePaymentMethod);

            if (expenseEntity == null)
            {
                return null;
            }

            return this.mapper.Map<ExpenseListDto>(expenseEntity);
        }

        public async Task AddExpenseAsync(CreateExpenseDto expenseDto)
        {
            if(expenseDto == null)
            {
                throw new ArgumentNullException(nameof(expenseDto));
            }

            var expenseEntity = this.mapper.Map<Expense>(expenseDto);
            await this.expenseRepository.AddExpenseAsync(expenseEntity);
            await this.expenseRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            var expenseEntityToDelete = await this.expenseRepository.GetExpenseByIdAsync(expenseId, false, false);
            
            if(expenseEntityToDelete == null)
            {
                return false;
            }

            this.expenseRepository.DeleteExpenseAsync(expenseEntityToDelete);
            await this.expenseRepository.SaveChangesAsync();
            return true;
        }
    }
}
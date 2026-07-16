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

        public async Task<IEnumerable<ExpenseListDto>> GetAllExpensesAsync(ExpenseFilterDto filters)
        {
            if (!ValidateFilters(filters))
            {
                throw new ArgumentException(nameof(filters));
            }

            var expenseEntities = await expenseRepository.GetExpensesAsync(filters);
            return mapper.Map<IEnumerable<ExpenseListDto>>(expenseEntities);
        }

        public async Task<ExpenseListDto?> GetExpenseByIdAsync(int id)
        {
            var expenseEntity = await expenseRepository.GetExpenseByIdAsync(id);

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

            var createdExpense = await expenseRepository.GetExpenseByIdAsync(expenseEntity.Id);

            return mapper.Map<ExpenseListDto>(createdExpense);
        }

        public async Task<ExpenseListDto?> UpdateExpenseAsync(
            int id,
            UpdateExpenseDto updateExpenseDto)
        {
            var expenseEntity = await expenseRepository.GetExpenseByIdAsync(id);

            if (expenseEntity == null)
            {
                return null;
            }

            mapper.Map(updateExpenseDto, expenseEntity);
            await expenseRepository.SaveChangesAsync();

            return mapper.Map<ExpenseListDto>(expenseEntity);
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            var expenseEntityToDelete = await expenseRepository.GetExpenseByIdAsync(expenseId);

            if (expenseEntityToDelete == null)
            {
                return false;
            }

            expenseRepository.DeleteExpenseAsync(expenseEntityToDelete);
            await expenseRepository.SaveChangesAsync();
            return true;
        }

        private bool ValidateFilters(ExpenseFilterDto filters)
        {
            if (filters.FromDate.HasValue
                && filters.ToDate.HasValue
                && filters.FromDate > filters.ToDate)
            {
                return false;
            }

            return true;
        }
    }
}
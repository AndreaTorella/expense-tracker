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

        public async Task<PagedResultDto<ExpenseListDto>> GetAllExpensesAsync(ExpenseFilterDto filters)
        {
            var result = await expenseRepository.GetExpensesAsync(filters);

            return new PagedResultDto<ExpenseListDto>
            {
                Items = mapper.Map<IEnumerable<ExpenseListDto>>(result.Items),
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize,
                TotalItems = result.TotalItems
            };
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
    }
}
using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Domain.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Repositories;

namespace ExpensesTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            this.transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<PagedResultDto<TransactionListDto>> GetAllTransactionsAsync(TransactionFilterDto filters)
        {
            var householdId = await this.currentUserService.GetHouseholdIdAsync();

            var filtersQuery = this.mapper.Map<TransactionQuery>(filters);
            var result = await transactionRepository.GetTransactionAsync(filtersQuery, householdId);

            return new PagedResultDto<TransactionListDto>
            {
                Items = mapper.Map<IEnumerable<TransactionListDto>>(result.Items),
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize,
                TotalItems = result.TotalItems
            };
        }

        public async Task<TransactionListDto?> GetTransactionByIdAsync(int id)
        {
            var householdId = await this.currentUserService.GetHouseholdIdAsync();
            var transactionEntity = await transactionRepository.GetTransactionByIdAsync(id, householdId);

            if (transactionEntity == null)
            {
                return null;
            }

            return mapper.Map<TransactionListDto>(transactionEntity);
        }

        public async Task<TransactionListDto> AddTransactionAsync(CreateTransactionDto transactionDto)
        {
            ArgumentNullException.ThrowIfNull(transactionDto);

            var householdId = await this.currentUserService.GetHouseholdIdAsync();
            var category = await this.categoryRepository.GetCategoryByIdAsync(transactionDto.CategoryId, householdId) ?? throw new ArgumentException("Category not valid");

            //Business rule
            if (transactionDto.TransactionType != category.TransactionType)
            {
                throw new ArgumentException("Category not compatible with the transaction");
            }

            var transactionEntity = mapper.Map<Transaction>(transactionDto);

            transactionEntity.HouseholdId = householdId;
            transactionEntity.CreatedByUserId = this.currentUserService.UserId;

            await transactionRepository.AddTransactionAsync(transactionEntity);
            await transactionRepository.SaveChangesAsync();

            var createdTransaction =
                await transactionRepository.GetTransactionByIdAsync(
                    transactionEntity.Id,
                    householdId);

            return mapper.Map<TransactionListDto>(createdTransaction);
        }

        public async Task<TransactionListDto?> UpdateTransactionAsync(
            int id,
            UpdateTransactionDto updateTransactionDto)
        {
            var householdId = await this.currentUserService.GetHouseholdIdAsync();
            var transactionEntity = await transactionRepository.GetTransactionByIdAsync(id, householdId);

            if (transactionEntity == null)
            {
                return null;
            }

            var category = await this.categoryRepository.GetCategoryByIdAsync(updateTransactionDto.CategoryId, householdId);

            if (category == null)
            {
                throw new ArgumentException("Category not valid");
            }

            if (updateTransactionDto.TransactionType != category.TransactionType)
            {
                throw new ArgumentException("Category not compatible with the transaction");
            }

            mapper.Map(updateTransactionDto, transactionEntity);
            await transactionRepository.SaveChangesAsync();

            return mapper.Map<TransactionListDto>(transactionEntity);
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            var householdId = await this.currentUserService.GetHouseholdIdAsync();
            var transactionEntityToDelete = await transactionRepository.GetTransactionByIdAsync(transactionId, householdId);

            if (transactionEntityToDelete == null)
            {
                return false;
            }

            transactionRepository.DeleteTransactionAsync(transactionEntityToDelete);
            await transactionRepository.SaveChangesAsync();
            return true;
        }
    }
}

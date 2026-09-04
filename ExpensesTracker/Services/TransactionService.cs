using AutoMapper;
using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<ApplicationUser> userManager;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager)
        {
            this.transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<PagedResultDto<TransactionListDto>> GetAllTransactionsAsync(TransactionFilterDto filters)
        {
            var currentUserId = this.currentUserService.UserId;
            var currentUser = await userManager.FindByIdAsync(currentUserId);

            if (currentUser == null)
            {
                throw new InvalidOperationException("Current user not found.");
            }

            var householdId = currentUser.HouseholdId;

            var result = await transactionRepository.GetTransactionAsync(filters, householdId);

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
            var transactionEntity = await transactionRepository.GetTransactionByIdAsync(id);

            if (transactionEntity == null)
            {
                return null;
            }

            return mapper.Map<TransactionListDto>(transactionEntity);
        }

        public async Task<TransactionListDto> AddTransactionAsync(CreateTransactionDto transactionDto)
        {
            if (transactionDto == null)
            {
                throw new ArgumentNullException(nameof(transactionDto));
            }

            var category = await this.categoryRepository.GetCategoryByIdAsync(transactionDto.CategoryId);

            if (category == null)
            {
                throw new ArgumentException("Category not valid");
            }

            //Business rule
            if (transactionDto.TransactionType != category.TransactionType)
            {
                throw new ArgumentException("Category not compatible with the transaction");
            }

            var transactionEntity = mapper.Map<Transaction>(transactionDto);

            transactionEntity.CreatedByUserId = currentUserService.UserId;

            await transactionRepository.AddTransactionAsync(transactionEntity);
            await transactionRepository.SaveChangesAsync();

            var createdTransaction = await transactionRepository.GetTransactionByIdAsync(transactionEntity.Id);

            return mapper.Map<TransactionListDto>(createdTransaction);
        }

        public async Task<TransactionListDto?> UpdateTransactionAsync(
            int id,
            UpdateTransactionDto updateTransactionDto)
        {
            var transactionEntity = await transactionRepository.GetTransactionByIdAsync(id);

            if (transactionEntity == null)
            {
                return null;
            }

            var category = await this.categoryRepository.GetCategoryByIdAsync(updateTransactionDto.CategoryId);

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
            var transactionEntityToDelete = await transactionRepository.GetTransactionByIdAsync(transactionId);

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

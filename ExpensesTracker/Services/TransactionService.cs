using AutoMapper;
using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Repositories;

namespace ExpensesTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper mapper;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            this.transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResultDto<TransactionListDto>> GetAllTransactionsAsync(TransactionFilterDto filters)
        {
            var result = await transactionRepository.GetTransactionAsync(filters);

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

            var transactionEntity = mapper.Map<Transaction>(transactionDto);
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

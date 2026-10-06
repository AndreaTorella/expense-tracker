using ExpensesTracker.Application.Common;
using ExpensesTracker.Application.Models;
using FluentValidation;

namespace ExpensesTracker.Application.Services
{
    public class ValidatingTransactionService : ITransactionService
    {
        private readonly ITransactionService inner;
        private readonly IValidator<CreateTransactionCommand> createValidator;
        private readonly IValidator<UpdateTransactionCommand> updateValidator;


        public ValidatingTransactionService(
            ITransactionService inner,
            IValidator<CreateTransactionCommand> createValidator,
            IValidator<UpdateTransactionCommand> updateValidator)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
            this.createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            this.updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        }

        public Task<PagedResult<TransactionResult>> GetAllTransactionsAsync(TransactionQuery filters)
        {
            return this.inner.GetAllTransactionsAsync(filters);
        }

        public Task<TransactionResult?> GetTransactionByIdAsync(int id)
        {
            return this.inner.GetTransactionByIdAsync(id);
        }

        public async Task<TransactionResult> AddTransactionAsync(CreateTransactionCommand transactionDto)
        {
            await this.createValidator.ValidateAndThrowAsync(transactionDto);
            return await this.inner.AddTransactionAsync(transactionDto);
        }
        public async Task<TransactionResult?> UpdateTransactionAsync(int id, UpdateTransactionCommand updateTransactionCommand)
        {
            await this.updateValidator.ValidateAndThrowAsync(updateTransactionCommand);
            return await this.inner.UpdateTransactionAsync(id, updateTransactionCommand);
        }

        public Task<bool> DeleteTransactionAsync(int transactionId)
        {
            return this.inner.DeleteTransactionAsync(transactionId);
        }
    }
}

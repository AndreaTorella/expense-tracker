using ExpensesTracker.Application.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExpensesTracker.Infrastructure.Persistence
{
    public class EfTransactionManager : IDbTransactionManager
    {
        private readonly ExpenseTrackerDbContext context;
        private IDbContextTransaction? currentTransaction;

        public EfTransactionManager(
            ExpenseTrackerDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task BeginTransactionAsync()
        {
            currentTransaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction has been started.");
            }

            await currentTransaction.CommitAsync();
            await currentTransaction.DisposeAsync();

            currentTransaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (currentTransaction == null)
            {
                return;
            }

            await currentTransaction.RollbackAsync();
            await currentTransaction.DisposeAsync();

            currentTransaction = null;
        }
    }
}

namespace ExpensesTracker.Application.Services
{
    public interface IDbTransactionManager
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}

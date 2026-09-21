namespace ExpensesTracker.Application.Services
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        Task<int> GetHouseholdIdAsync();
    }
}

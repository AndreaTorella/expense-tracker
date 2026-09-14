namespace ExpensesTracker.Services
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        Task<int> GetHouseholdIdAsync();
    }
}

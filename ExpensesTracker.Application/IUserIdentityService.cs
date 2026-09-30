using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application
{
    public interface IUserIdentityService
    {
        Task<UserCreationResult> CreateUserAsync(
            string email,
            string password,
            int householdId);
    }
}

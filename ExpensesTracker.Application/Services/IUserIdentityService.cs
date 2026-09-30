using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface IUserIdentityService
    {
        Task<UserCreationResult> CreateUserAsync(
            string email,
            string password,
            int householdId);

        Task<AuthenticatedUserResult?> ValidateCredentialsAsync(
            string email,
            string password);
    }
}

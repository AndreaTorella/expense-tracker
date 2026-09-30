using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResult?> LoginAsync(LoginCommand command);
    }
}

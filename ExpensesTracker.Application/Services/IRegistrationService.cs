using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationResult> RegisterAsync(RegisterCommand registerCommand);
    }
}

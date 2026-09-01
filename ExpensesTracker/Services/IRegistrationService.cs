using ExpensesTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Services
{
    public interface IRegistrationService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
    }
}

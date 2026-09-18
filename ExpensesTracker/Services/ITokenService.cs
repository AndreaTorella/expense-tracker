using ExpensesTracker.Infrastructure.Identity;

namespace ExpensesTracker.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}

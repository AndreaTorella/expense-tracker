using ExpensesTracker.Entities;

namespace ExpensesTracker.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}

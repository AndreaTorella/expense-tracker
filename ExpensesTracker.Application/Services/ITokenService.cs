using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface ITokenService
    {
        string CreateToken(AuthenticatedUserResult user);
    }
}

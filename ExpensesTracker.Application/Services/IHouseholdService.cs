using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public interface IHouseholdService
    {
        Task<HouseholdResult> AddHouseholdAsync(CreateHouseholdCommand household);
    }
}

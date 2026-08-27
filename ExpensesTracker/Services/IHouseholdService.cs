using ExpensesTracker.Models;

namespace ExpensesTracker.Services
{
    public interface IHouseholdService
    {
        Task<HouseholdDto> AddHouseholdAsync(CreateHouseholdDto household);
    }
}

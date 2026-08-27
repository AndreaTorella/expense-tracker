using ExpensesTracker.Entities;

namespace ExpensesTracker.Repositories
{
    public interface IHouseholdRepository
    {
        Task AddHouseholdAsync(Household household);
        Task SaveChangesAsync();
    }
}

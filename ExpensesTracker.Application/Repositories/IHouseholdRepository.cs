using ExpensesTracker.Domain.Entities;

namespace ExpensesTracker.Application.Repositories
{
    public interface IHouseholdRepository
    {
        Task AddHouseholdAsync(Household household);
        Task SaveChangesAsync();
    }
}

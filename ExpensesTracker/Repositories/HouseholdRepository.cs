using ExpensesTracker.Data;
using ExpensesTracker.Entities;

namespace ExpensesTracker.Repositories
{
    public class HouseholdRepository : IHouseholdRepository
    {
        private readonly ExpenseTrackerDbContext context;

        public HouseholdRepository(ExpenseTrackerDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddHouseholdAsync(Household household)
        {
            if (household == null)
            {
                throw new ArgumentNullException(nameof(household));
            }

            await this.context.Households.AddAsync(household);
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}

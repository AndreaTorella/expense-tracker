using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int HouseholdId { get; set; }
        public Household Household { get; set; } = null!;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

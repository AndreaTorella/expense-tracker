using ExpensesTracker.Domain.Entities;
using ExpensesTracker.Domain.Enums;
using ExpensesTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Infrastructure.Persistence
{
    public class ExpenseTrackerDbContext : IdentityDbContext<ApplicationUser>
    {
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Household> Households { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Con IdentityDbContext configura lo schema identity

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseTrackerDbContext).Assembly);

            // SEED PAYMENT METHODS
            modelBuilder.Entity<PaymentMethod>().HasData(
                Enum.GetValues(typeof(PaymentMethodName))
                    .Cast<PaymentMethodName>()
                    .Select((e, index) => new PaymentMethod
                    {
                        Id = index + 1,
                        Name = e
                    })
            );
        }
    }
}

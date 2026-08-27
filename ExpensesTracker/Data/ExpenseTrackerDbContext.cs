using ExpensesTracker.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Data
{
    public class ExpenseTrackerDbContext : IdentityDbContext<ApplicationUser>
    {
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Household> Household { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Con IdentityDbContext configura lo schema identity

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseTrackerDbContext).Assembly);

            // SEED CATEGORIES
            modelBuilder.Entity<Category>().HasData(
                Enum.GetValues(typeof(CategoryName))
                    .Cast<CategoryName>()
                    .Select((categoryName, index) => new Category
                    {
                        Id = index + 1,
                        Name = categoryName,
                        TransactionType = categoryName switch
                        {
                            CategoryName.Salary
                                or CategoryName.Bonus
                                or CategoryName.Refund
                                or CategoryName.Gift
                                    => TransactionType.Income,

                            _ => TransactionType.Expense
                        }
                    })
            );

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

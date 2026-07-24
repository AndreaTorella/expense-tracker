using ExpensesTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Data
{
    public class ExpenseTrackerDbContext : DbContext
    {
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseTrackerDbContext).Assembly);

            // SEED CATEGORIES
            modelBuilder.Entity<Category>().HasData(
                Enum.GetValues(typeof(CategoryName))
                    .Cast<CategoryName>()
                    .Select((e, index) => new Category
                    {
                        Id = index + 1,
                        Name = e
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

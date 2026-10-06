using ExpensesTracker.Application.Repositories;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Infrastructure.Identity;
using ExpensesTracker.Infrastructure.Persistence;
using ExpensesTracker.Infrastructure.Repositories;
using ExpensesTracker.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ExpenseTrackerDbContext>(options =>
                options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<ExpenseTrackerDbContext>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IHouseholdRepository, HouseholdRepository>();

            services.AddScoped<IUserIdentityService, UserIdentityService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDbTransactionManager, EfTransactionManager>();

            return services;
        }
    }
}

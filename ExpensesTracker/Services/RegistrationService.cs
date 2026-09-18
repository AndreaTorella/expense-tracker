using ExpensesTracker.Infrastructure.Data;
using ExpensesTracker.Infrastructure.Identity;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Services
{
    /*
        Orchestration service beacuse the registration involves both Household and Identity.
        Example: if it's not possible to create an user beacuse the Identity rules are not respected, then I can have an orphan household 
    */
    public class RegistrationService : IRegistrationService
    {
        private readonly IHouseholdService householdService;
        private readonly ExpenseTrackerDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public RegistrationService(
            IHouseholdService household,
            ExpenseTrackerDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.householdService = household ?? throw new ArgumentNullException(nameof(householdService));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            await using var transaction = await this.context.Database.BeginTransactionAsync();

            try
            {
                var createHouseholdDto = new CreateHouseholdDto
                {
                    FamilyName = registerDto.FamilyName
                };

                var household = await this.householdService.AddHouseholdAsync(createHouseholdDto);

                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    HouseholdId = household.Id,
                };

                var result = await userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return result;
                }

                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

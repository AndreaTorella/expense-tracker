using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Infrastructure.Identity;
using ExpensesTracker.Infrastructure.Persistence;
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
            IHouseholdService householdService,
            ExpenseTrackerDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.householdService = householdService ?? throw new ArgumentNullException(nameof(householdService));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<RegistrationResult> RegisterAsync(RegisterCommand registerDto)
        {
            await using var transaction = await this.context.Database.BeginTransactionAsync();

            try
            {
                var createHouseholdCommand = new CreateHouseholdCommand
                {
                    FamilyName = registerDto.FamilyName
                };

                var household = await this.householdService.AddHouseholdAsync(createHouseholdCommand);

                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    HouseholdId = household.Id,
                };

                var identityResult = await userManager.CreateAsync(user, registerDto.Password);

                var registrationResult = new RegistrationResult
                {
                    Succeeded = identityResult.Succeeded,
                    Errors = identityResult.Errors.Select(x => new RegistrationError { Code = x.Code, Description = x.Description })
                };

                if (!registrationResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return registrationResult;
                }

                await transaction.CommitAsync();

                return registrationResult;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

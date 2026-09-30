using ExpensesTracker.Application;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Infrastructure.Persistence;

namespace ExpensesTracker.Services
{
    /*
        Orchestration service beacuse the registration involves both Household and Identity.
        Example: if it's not possible to create an user beacuse the Identity rules are not respected, then I can have an orphan household 
    */
    public class RegistrationService : IRegistrationService
    {
        private readonly IHouseholdService householdService;
        private readonly IUserIdentityService userIdentityService;
        private readonly ExpenseTrackerDbContext context;

        public RegistrationService(
            IHouseholdService householdService,
            IUserIdentityService userIdentityService,
            ExpenseTrackerDbContext context)
        {
            this.householdService = householdService ?? throw new ArgumentNullException(nameof(householdService));
            this.userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<RegistrationResult> RegisterAsync(RegisterCommand registerCommand)
        {
            await using var transaction = await this.context.Database.BeginTransactionAsync();

            try
            {
                var createHouseholdCommand = new CreateHouseholdCommand
                {
                    FamilyName = registerCommand.FamilyName
                };

                var household = await this.householdService.AddHouseholdAsync(createHouseholdCommand);

                var userCreationResult = await this.userIdentityService.CreateUserAsync(
                    registerCommand.Email,
                    registerCommand.Password,
                    household.Id);

                var registrationResult = new RegistrationResult
                {
                    Succeeded = userCreationResult.Succeeded,
                    Errors = [.. userCreationResult.Errors.Select(x => new RegistrationError { Code = x.Code, Description = x.Description })]
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

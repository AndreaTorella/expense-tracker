using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    /*
        Orchestration service beacuse the registration involves both Household and Identity.
        Example: if it's not possible to create an user beacuse the Identity rules are not respected, then I can have an orphan household 
    */
    public class RegistrationService : IRegistrationService
    {
        private readonly IHouseholdService householdService;
        private readonly IUserIdentityService userIdentityService;
        private readonly IDbTransactionManager transactionManager;

        public RegistrationService(
            IHouseholdService householdService,
            IUserIdentityService userIdentityService,
            IDbTransactionManager transactionManager)
        {
            this.householdService = householdService ?? throw new ArgumentNullException(nameof(householdService));
            this.userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
            this.transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
        }

        public async Task<RegistrationResult> RegisterAsync(RegisterCommand registerCommand)
        {
            await this.transactionManager.BeginTransactionAsync();

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
                    await this.transactionManager.RollbackTransactionAsync();
                    return registrationResult;
                }

                await this.transactionManager.CommitTransactionAsync();
                return registrationResult;
            }
            catch
            {
                await this.transactionManager.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

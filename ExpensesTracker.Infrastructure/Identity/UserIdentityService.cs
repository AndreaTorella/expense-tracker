using ExpensesTracker.Application;
using ExpensesTracker.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Infrastructure.Identity
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserIdentityService(

            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<UserCreationResult> CreateUserAsync(string email, string password, int householdId)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                HouseholdId = householdId,
            };

            var identityResult = await this.userManager.CreateAsync(user, password);

            return new UserCreationResult
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors
                    .Select(error => new UserIdentityError
                    {
                        Code = error.Code,
                        Description = error.Description
                    })
                    .ToList()
            };
        }
    }
}

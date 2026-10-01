using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
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
                Errors = [.. identityResult.Errors
                    .Select(error => new UserIdentityError
                    {
                        Code = error.Code,
                        Description = error.Description
                    })]
            };
        }

        public async Task<AuthenticatedUserResult?> ValidateCredentialsAsync(string email, string password)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var isPasswordValid = await this.userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
            {
                return null;
            }

            return new AuthenticatedUserResult
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                HouseholdId = user.HouseholdId,
            };
        }
    }
}

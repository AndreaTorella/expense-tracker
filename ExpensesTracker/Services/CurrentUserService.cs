using ExpensesTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public string UserId
        {
            get
            {
                var userId = httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new InvalidOperationException("Current user id is not available.");
                }

                return userId;
            }
        }

        public async Task<int> GetHouseholdIdAsync()
        {
            var currentUserId = this.UserId;
            var currentUser = await this.userManager.FindByIdAsync(currentUserId);

            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            return currentUser.HouseholdId;
        }
    }
}

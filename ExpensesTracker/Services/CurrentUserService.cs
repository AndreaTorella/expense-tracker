using ExpensesTracker.Application.Services;

namespace ExpensesTracker.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserIdentityService userIdentityService;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
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
            var householdId = await this.userIdentityService.GetHouseholdIdByUserIdAsync(this.UserId);

            if (householdId == null)
            {
                throw new ArgumentNullException(nameof(householdId));
            }

            return householdId.Value;
        }
    }
}

using ExpensesTracker.Application.Services;

namespace ExpensesTracker.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private const string HouseholdIdClaim = "household_id";
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
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

        public int HouseholdId
        {
            get
            {
                var householdIdClaim = this.httpContextAccessor.HttpContext?
                    .User
                    .FindFirst(HouseholdIdClaim)?
                    .Value;

                if (!int.TryParse(householdIdClaim, out var householdId))
                {
                    throw new InvalidOperationException("Current user household id is not available.");
                }

                return householdId;
            }
        }
    }
}

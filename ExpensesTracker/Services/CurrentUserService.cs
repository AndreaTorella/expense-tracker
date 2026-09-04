namespace ExpensesTracker.Services
{
    public class CurrentUserService : ICurrentUserService
    {
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
    }
}

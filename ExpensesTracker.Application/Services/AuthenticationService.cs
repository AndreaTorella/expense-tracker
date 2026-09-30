using ExpensesTracker.Application.Models;

namespace ExpensesTracker.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserIdentityService userIdentityService;
        private readonly ITokenService tokenService;

        public AuthenticationService(
            IUserIdentityService userIdentityService,
            ITokenService tokenService)
        {
            this.userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<LoginResult?> LoginAsync(LoginCommand command)
        {
            var authenticatedUser = await this.userIdentityService.ValidateCredentialsAsync(command.Email, command.Password);

            if (authenticatedUser == null)
            {
                return null;
            }

            var token = this.tokenService.CreateToken(authenticatedUser);

            return new LoginResult
            {
                Token = token,
            };
        }
    }
}

using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IMapper mapper;
        private readonly IRegistrationService registrationService;
        private readonly ITokenService tokenService;
        private readonly IUserIdentityService userIdentityService;

        public AuthController(
            IMapper mapper,
            IRegistrationService registrationService,
            ITokenService tokenService,
            IUserIdentityService userIdentityService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                throw new ArgumentNullException(nameof(registerDto));
            }

            var registerCommand = this.mapper.Map<RegisterCommand>(registerDto);
            var registrationResult = await this.registrationService.RegisterAsync(registerCommand);

            if (!registrationResult.Succeeded)
            {
                return BadRequest(registrationResult.Errors);
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            var authenticatedUser = await this.userIdentityService.ValidateCredentialsAsync(
                loginDto.Email,
                loginDto.Password);

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            var token = tokenService.CreateToken(authenticatedUser);

            return Ok(new
            {
                token
            });
        }
    }
}

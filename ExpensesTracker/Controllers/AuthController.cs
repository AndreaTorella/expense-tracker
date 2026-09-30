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
        private readonly IAuthenticationService authenticationService;

        public AuthController(
            IMapper mapper,
            IRegistrationService registrationService,
            IAuthenticationService authenticationService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            ArgumentNullException.ThrowIfNull(registerDto);

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
            ArgumentNullException.ThrowIfNull(loginDto);

            var loginCommand = new LoginCommand
            {
                Email = loginDto.Email,
                Password = loginDto.Password,
            };

            var loginResult = await this.authenticationService.LoginAsync(loginCommand);

            if (loginResult == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                loginResult.Token
            });
        }
    }
}

using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Infrastructure.Identity;
using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> userManager;

        public AuthController(
            IMapper mapper,
            IRegistrationService registrationService,
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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

            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
            {
                return Unauthorized();
            }

            var token = tokenService.CreateToken(user);

            return Ok(new
            {
                token
            });
        }
    }
}

using ExpensesTracker.Entities;
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
        private readonly IRegistrationService registrationService;
        private readonly ITokenService tokenService;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthController(
            IRegistrationService registrationService,
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager)
        {
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

            var result = await this.registrationService.RegisterAsync(registerDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
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

using ExpensesTracker.Entities;
using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpensesTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ITokenService tokenService;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthController(
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager)
        {
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

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            if(loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            var user = await this.userManager.FindByEmailAsync(loginDto.Email);

            if(user == null)
            {
                return Unauthorized();
            }

            var isPasswordValid = await this.userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
            {
                return Unauthorized();
            }

            var token = this.tokenService.CreateToken(user);

            return Ok(new
            {
                token
            });
        }
    }
}

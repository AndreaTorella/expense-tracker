using ExpensesTracker.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpensesTracker.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateToken(ApplicationUser user)
        {
            var jwtKey = configuration["jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured");
            var issuer = configuration["jwt:Issuer"];
            var audience = configuration["jwt:Audience"];
            var expirationMinutes = int.Parse(configuration["jwt:ExpirationMinutes"] ?? "60");

            var claims = new List<Claim> //JWT payload
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

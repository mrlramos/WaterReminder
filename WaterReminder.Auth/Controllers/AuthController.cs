using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WaterReminder.Auth.Models;

namespace WaterReminder.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            // Validação simples de usuário (substitua pela sua lógica)
            if (login.Username != "usuario" || login.Password != "senha")
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            string? jwtSecret = configuration.GetValue("JWT_SECRET", string.Empty);
            Console.WriteLine(jwtSecret);

            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new Exception("JWT_SECRET nao configurado");
            }

            byte[] key = Encoding.ASCII.GetBytes(jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                Token = tokenHandler.WriteToken(token)
            });
        }
    }
}

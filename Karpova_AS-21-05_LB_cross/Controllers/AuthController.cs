using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Karpova_AS_21_05_LB_cross.Models; // Подключите модель LoginDto, если она в этом пространстве имен

namespace Karpova_AS_21_05_LB_cross.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Пример: проверка логина и пароля (замените на свою логику)
            if (loginDto.Username == "admin" && loginDto.Password == "admin12345")
            {
                var token = GenerateJwtToken(loginDto.Username);
                return Ok(new { token });
            }

            else if (loginDto.Username == "user" && loginDto.Password == "user12345")
            {
                var token = GenerateJwtToken(loginDto.Username);
                return Ok(new { token });
            }

            return Unauthorized(); // Неверные логин/пароль
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, username == "admin" ? "Admin" : "User") // Пример роли
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

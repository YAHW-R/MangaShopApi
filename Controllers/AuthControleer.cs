using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MangaShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            // 1. Validar credenciales contra el appsettings (Sin Base de Datos)
            var adminUser = _configuration["AdminSettings:Username"];
            var adminPass = _configuration["AdminSettings:Password"];

            if (login.Username == adminUser && login.Password == adminPass)
            {
                // 2. Crear los Claims (datos dentro del token)
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                // 3. Crear la llave de seguridad
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // 4. Generar el Token
                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1), // El token dura 1 hora
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return login.Username != adminUser ? Unauthorized("Incorrect Username") : Unauthorized("Incorrect Password");
        }


        [HttpGet("validate")]
        [Authorize]
        public IActionResult Validate()
        {
            return Ok();
        }
    }

    // Clase simple para recibir los datos
    public class LoginModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
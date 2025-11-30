using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Minimarket.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Authentication(UserLogin userLogin)
        {
            //Si es un usuario válido
            var token = GenerateToken(userLogin);
            return Ok(new { token });
        }

        private string GenerateToken(UserLogin userLogin)
        {
            //Header
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims (Cuerpo)
            var claims = new[]
            {
            new Claim("Name", "Juan Perez"),
            new Claim(ClaimTypes.Email, "jperez@ucb.edu.bo"),
            new Claim(ClaimTypes.Role, "Administrador"),
        };

            //Payload
            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(2)
            );

            //Generar el token JWT
            var token = new JwtSecurityToken(header, payload);

            //Serializar el token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

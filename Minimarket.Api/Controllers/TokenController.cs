using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Minimarket.Core.Data;
using Minimarket.Core.Entities;
using Minimarket.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        public TokenController(IConfiguration configuration, ISecurityService securityService)
        {
            _configuration = configuration;
            _securityService = securityService;
        }
        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin userLogin)
        {
            //Si es un usuario válido
            var validation = await IsValidUser(userLogin);
            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);
                return Ok(new { token });
            }

            return NotFound();
        }

        private async Task<(bool, Security)> IsValidUser(UserLogin login)
        {
            var user = await _securityService.GetLoginByCredentials(login);
            return (user != null, user);
        }


        private string GenerateToken(Security security)
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
            new Claim("Login", security.Login),
            new Claim("Name", security.Name),
            new Claim(ClaimTypes.Role, security.Role.ToString()),
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

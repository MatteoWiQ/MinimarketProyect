using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Minimarket.Core.Data;
using Minimarket.Core.Entities;
using Minimarket.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;

        public TokenController(IConfiguration configuration, ISecurityService securityService, IPasswordService passwordService)
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Genera un token JWT a partir de las credenciales del usuario.
        /// </summary>
        /// <remarks>
        /// Envía un objeto UserLogin con Login y Password para autenticar.
        /// Si las credenciales son válidas devuelve un token JWT con Claims.
        /// </remarks>
        /// <param name="userLogin">Credenciales del usuario.</param>
        /// <returns>Token JWT.</returns>
        /// <response code="200">Autenticación exitosa, token generado.</response>
        /// <response code="404">Credenciales inválidas.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authentication(UserLogin userLogin)
        {
            try
            {
                var validation = await IsValidUser(userLogin);
                if (validation.Item1)
                {
                    var token = GenerateToken(validation.Item2);
                    return Ok(new { token });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Verifica si las credenciales del usuario son válidas.
        /// </summary>
        /// <param name="login">Credenciales ingresadas.</param>
        /// <returns>Tupla indicando si es válido y el usuario encontrado.</returns>
        private async Task<(bool, Security)> IsValidUser(UserLogin login)
        {
            var user = await _securityService.GetLoginByCredentials(login);
            var isValid = _passwordService.Check(user.Password, login.Password);
            return (isValid, user);
        }

        /// <summary>
        /// Genera un token JWT con la información del usuario autenticado.
        /// </summary>
        /// <param name="security">Datos del usuario.</param>
        /// <returns>Token JWT serializado.</returns>
        private string GenerateToken(Security security)
        {
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            var claims = new[]
            {
                new Claim("Login", security.Login),
                new Claim("Name", security.Name),
                new Claim(ClaimTypes.Role, security.Role.ToString()),
            };

            var expirationMinutes = Convert.ToDouble(_configuration["Authentication:ExpirationMinutes"]);

            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Permite verificar rápidamente las conexiones configuradas (MySQL y SQL Server).
        /// </summary>
        /// <returns>Lista con los connection strings activos.</returns>
        /// <response code="200">Conexiones leídas correctamente.</response>
        [HttpGet("TestConeccion")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> TestConexion()
        {
            var result = new
            {
                ConnectionMySql = _configuration["ConnectionStrings:ConnectionMySql"],
                ConnectionSqlServer = _configuration["ConnectionStrings:ConnectionSqlServer"]
            };

            return Ok(result);
        }

        /// <summary>
        /// Permite ver la configuración de las cadenas de conexion y autenticación. 
        /// </summary>
        /// <returns> Retorna la configuración actual de conexión y autenticación. </returns>
        /// <response code="200">Configuración leída correctamente.</response>
        /// <response code="500">Error interno del servidor.</response> 

        [HttpGet("Config")]
        public async Task<IActionResult> GetConfig()
        {
            try
            {
                var connectionStringMySql = _configuration["ConnectionStrings:ConnectionMySql"];
                var connectionStringSqlServer = _configuration["ConnectionStrings:ConnectionSqlServer"];

                var result = new
                {
                    connectionStringMySql = connectionStringMySql ?? "My SQL NO CONFIGURADO",
                    connectionStringSqlServer = connectionStringSqlServer ?? "SQL SERVER NO CONFIGURADO",
                    AllConnectionStrings = _configuration.GetSection("ConnectionStrings").GetChildren().Select(x => new { Key = x.Key, Value = x.Value }),
                    Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "ASPNETCORE_ENVIRONMENT NO CONFIGURADO",
                    Authentication = _configuration.GetSection("Authentication").GetChildren().Select(x => new { Key = x.Key, Value = x.Value })
                };

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

    }
}

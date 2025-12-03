using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.Entities;
using Minimarket.Core.Enum;
using Minimarket.Core.Interfaces;
using Minimarket.Infraestructure.Mappings;
using Minimarket.Infrastructure.Dtos;

namespace Minimarket.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IPasswordService _passwordService;
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;

        public SecurityController(ISecurityService securityService, IMapper mapper, IPasswordService passwordService)
        {
            _securityService = securityService;
            _mapper = mapper;
            _passwordService = passwordService;
        }
        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <remarks>
        /// Envía un objeto SecurityDto con Login, Name, Password y Role.
        /// El password se almacena hasheado.
        /// </remarks>
        /// <param name="securityDto">Datos del usuario a registrar.</param>
        /// <returns>Objeto ApiResponse con la información del usuario creado.</returns>
        /// <response code="200">Usuario registrado correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        //[Authorize(Roles = $"{nameof(RoleType.Administrator)}")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SecurityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);

            security.Password = _passwordService.Hash(securityDto.Password);
            await _securityService.RegisterUser(security);

            securityDto = _mapper.Map<SecurityDto>(security);
            var response = new ApiResponse<SecurityDto>(securityDto);
            return Ok(response);
        }
    }
}

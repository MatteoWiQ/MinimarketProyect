using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Dtos;
using Minimarket.Core.Enum;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interface;
using Minimarket.Core.QueryFilters;
using Minimarket.Infraestructure.Validations;
using System.Net;

namespace Minimarket.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidatorService _validatorService;

        public UserController(IUserService userService, IMapper mapper, IValidatorService validatorService)
        {
            _userService = userService;
            _mapper = mapper;
            _validatorService = validatorService;
        }

        /// <summary>
        /// Recupera una lista paginada de usuarios (DTO).
        /// </summary>
        /// <remarks>
        /// Puedes aplicar parámetros de filtro (paginación, nombre, email, rol, etc.).
        /// Devuelve un objeto ApiResponse con los usuarios y los metadatos de paginación.
        /// </remarks>
        /// <param name="filters">Filtros de consulta.</param>
        /// <returns>ApiResponse con IEnumerable&lt;UserDto&gt;.</returns>
        /// <response code="200">Operación exitosa.</response>
        /// <response code="400">Solicitud inválida.</response>
        /// <response code="500">Error interno.</response>
        [HttpGet("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUsersDtoMapper([FromQuery] UserQueryFilter filters)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(filters);

                var usersDto = _mapper.Map<IEnumerable<UserDto>>(users.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = users.Pagination.TotalCount,
                    PageSize = users.Pagination.PageSize,
                    CurrentPage = users.Pagination.CurrentPage,
                    TotalPages = users.Pagination.TotalPages,
                    HasNextPage = users.Pagination.HasNextPage,
                    HasPreviousPage = users.Pagination.HasPreviousPage
                };

                var response = new ApiResponse<IEnumerable<UserDto>>(usersDto)
                {
                    Pagination = pagination,
                    Messages = users.Messages
                };

                return StatusCode((int)users.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new[] { new Message { Type = TypeMessage.error.ToString(), Description = err.Message } }
                };
                return StatusCode(400, responsePost);
            }
        }

        /// <summary>
        /// Recupera un usuario por su Id.
        /// </summary>
        /// <param name="id">Id del usuario.</param>
        /// <returns>ApiResponse con UserDto.</returns>
        /// <response code="200">Operación exitosa.</response>
        /// <response code="404">Usuario no encontrado.</response>
        [HttpGet("dto/mapper/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserByIdDtoMapper(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                var userDto = _mapper.Map<UserDto>(user);
                var response = new ApiResponse<UserDto>(userDto);
                return Ok(response);
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        /// <summary>
        /// Inserta un nuevo usuario.
        /// </summary>
        /// <param name="userDto">DTO con los datos del usuario a registrar.</param>
        /// <returns>ApiResponse con el usuario creado.</returns>
        /// <response code="200">Usuario creado exitosamente.</response>
        /// <response code="400">Validación fallida o datos incorrectos.</response>
        /// <response code="409">Conflicto (usuario ya existe).</response>
        [HttpPost("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> InsertUserDtoMapper(UserDto userDto)
        {
            try
            {
                var validationResult = await _validatorService.ValidateAsync(userDto);
                if (!validationResult.IsValid)
                    return BadRequest(new { Errors = validationResult.Errors });

                var user = _mapper.Map<User>(userDto);
                await _userService.InsertAsync(user);
                userDto.Id = user.Id;

                var response = new ApiResponse<UserDto>(userDto);
                return Ok(response);
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        /// <summary>
        /// Actualiza un usuario por su Id.
        /// </summary>
        /// <param name="id">Id del usuario a actualizar.</param>
        /// <param name="userDto">DTO con la información modificada.</param>
        /// <returns>ApiResponse con el usuario actualizado.</returns>
        /// <response code="200">Actualización exitosa.</response>
        /// <response code="400">Id no coincide o datos incorrectos.</response>
        /// <response code="404">Usuario no encontrado.</response>
        [HttpPut("dto/mapper/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<User>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateUserDtoMapper(int id, UserDto userDto)
        {
            try
            {
                if (id != userDto.Id)
                    throw new BussinesException("El ID del usuario no coincide", (int)HttpStatusCode.BadRequest);

                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                    throw new BussinesException("Usuario no encontrado", (int)HttpStatusCode.NotFound);

                _mapper.Map(userDto, user);
                await _userService.UpdateAsync(user);

                var response = new ApiResponse<User>(user);
                return Ok(response);
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        /// <summary>
        /// Elimina un usuario por Id.
        /// </summary>
        /// <param name="id">Id del usuario a eliminar.</param>
        /// <response code="204">Eliminación exitosa.</response>
        /// <response code="404">Usuario no encontrado.</response>
        [HttpDelete("dto/mapper/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteUserDtoMapper(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        /// <summary>
        /// Endpoint de prueba para verificar el estado del controlador de usuarios.
        /// </summary>
        [HttpGet("Test")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Test()
        {
            return Ok(new { status = "UserController OK" });
        }
    }
}

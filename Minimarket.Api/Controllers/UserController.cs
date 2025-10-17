using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Hosting;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Interface;
using Minimarket.Core.Validator;
using Minimarket.Api.Responses;
using Minimarket.Infraestructure.Validations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> getUsersDtoMapper()
        {
            var users = await _userService.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> getUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            var userDto = _mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertUser([FromBody] UserDto userDto)
        {
            try
            {
                var validationResult = await _validatorService.ValidateAsync(userDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var user = _mapper.Map<User>(userDto);
                await _userService.InsertAsync(user);

                var response = new ApiResponse<User>(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest("El ID del usuario no coincide");
            else
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                    return NotFound("Usuario no encontrado");
                _mapper.Map(userDto, user);
                await _userService.UpdateAsync(user);

                var response = new ApiResponse<User>(user);
                return Ok(response);
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
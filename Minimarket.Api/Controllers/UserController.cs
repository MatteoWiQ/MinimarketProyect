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
using Minimarket.Infraestructure.Repositories;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.QueryFilters;
using Minimarket.Core.Interfaces;
using Minimarket.Core.Enum;
using System.Reflection.Metadata.Ecma335;

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
        public async Task<IActionResult> GetUsersDtoMapper([FromQuery]UserQueryFilter filters)
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

                    //statusCode = users.StatusCode,
                    Pagination = pagination,
                    Messages = users.Messages
                };

                return StatusCode((int)users.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(500, responsePost);
            }

        }



        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> getUserById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                var userDto = _mapper.Map<UserDto>(user);
                var response = new ApiResponse<UserDto>(userDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
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
                // retornar la excepcion controlada con el mensaje y su status code
                //

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
            try { 
                await _userService.DeleteAsync(id);
                // retornar un mensaje de exito no content
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }
    }
}
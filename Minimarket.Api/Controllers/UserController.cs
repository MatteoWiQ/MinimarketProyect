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
using Minimarket.Core.Exceptions;

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
                return StatusCode(400, responsePost);
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
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
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
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            try { 
                var user = await _userService.GetByIdAsync(id);
                if(id != userDto.Id)
                    throw new BussinesException("El ID del usuario no coincide con el ID de la ruta", 400);

                if (user == null)
                    throw new BussinesException("El usuario no existe.", 404);   

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

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try { 
                await _userService.DeleteAsync(id);
                
                return NoContent();
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
            
        }
        #region Dapper Queries
        [HttpGet("user-with-most-sales")]
        public async Task<IActionResult> GetUserWithMostSales()
        {
            try
            {
                var userWithMostSales = await _userService.GetUserWithMostSales();
                var response = new ApiResponse<UserWithMostSalesResponse>(userWithMostSales);
                response.Messages = new Message[] { new() { Type = TypeMessage.success.ToString(), Description = "Usuario con más ventas obtenido correctamente." } };
                return Ok(response);
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }   


        [HttpGet("age-users")]
        public async Task<IActionResult> GetAllAgeUsers([FromQuery] AgeUsersPaginationResponse filters)
        {
            try
            {
                var ageUsers = await _userService.GetAllAgeUsers(filters);
                var ageUsersDto = _mapper.Map<IEnumerable<AgeOfUsersResponse>>(ageUsers.Pagination);
                var pagination = new Pagination
                {
                    TotalCount = ageUsers.Pagination.TotalCount,
                    PageSize = ageUsers.Pagination.PageSize,
                    CurrentPage = ageUsers.Pagination.CurrentPage,
                    TotalPages = ageUsers.Pagination.TotalPages,
                    HasNextPage = ageUsers.Pagination.HasNextPage,
                    HasPreviousPage = ageUsers.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<AgeOfUsersResponse>>(ageUsersDto)
                {
                    Pagination = pagination,
                    Messages = ageUsers.Messages
                };
                return StatusCode((int)ageUsers.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(400, responsePost);
            }
        }

        [HttpGet("summarize-type-of-users")]
        public async Task<IActionResult> GetSummarizeTypeOfUsers([FromQuery] SummarizeUserTypePagination filters)
        {
            try
            {
                var summarizeTypeOfUsers = await _userService.GetSummarizeTypeOfUsers(filters);
                var summarizeTypeOfUsersDto = _mapper.Map<IEnumerable<SummarizeTypeOfUsers>>(summarizeTypeOfUsers.Pagination);
                var pagination = new Pagination
                {
                    TotalCount = summarizeTypeOfUsers.Pagination.TotalCount,
                    PageSize = summarizeTypeOfUsers.Pagination.PageSize,
                    CurrentPage = summarizeTypeOfUsers.Pagination.CurrentPage,
                    TotalPages = summarizeTypeOfUsers.Pagination.TotalPages,
                    HasNextPage = summarizeTypeOfUsers.Pagination.HasNextPage,
                    HasPreviousPage = summarizeTypeOfUsers.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<SummarizeTypeOfUsers>>(summarizeTypeOfUsersDto)
                {
                    Pagination = pagination,
                    Messages = summarizeTypeOfUsers.Messages
                };
                return StatusCode((int)summarizeTypeOfUsers.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(400, responsePost);
            }
        }
        #endregion
    }
}
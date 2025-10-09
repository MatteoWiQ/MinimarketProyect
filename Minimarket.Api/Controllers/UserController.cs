using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.Dtos;
using Minimarket.Core.Interface;
using Minimarket.Core.Validator;



namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly CreateUserValidator _userValidator;
        public UserController( IUserService userService, IMapper mapper, CreateUserValidator userValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _userValidator = userValidator;
        }
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> getUsersDtoMapper()
        {
            var users = await _userService.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto);
            return Ok(response);
        }
    }
}
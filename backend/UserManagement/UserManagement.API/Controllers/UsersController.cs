using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Dtos.Responses;
using UserManagement.Application.Services;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("/api/v1/users")]
    public class UsersController
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("registration")]
        public async Task Register([FromBody] UserRegistrationRequestDto request)
        {
            await _usersService.Register(request);
        }

        [HttpGet]
        [Authorize]
        public async Task<List<UserResponseDto>> GetUsers([FromQuery] string? userName)
        {
            return await _usersService.GetUsers(userName);
        }

        [HttpPost("login")]
        public async Task<AuthenticationResponseDto> Login([FromBody] AuthenticationRequestDto request)
        {
            return await _usersService.Login(request);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<UserResponseDto> GetUser([FromRoute] int id)
        {
            return await _usersService.GetUser(id);
        }

        [HttpGet("personal")]
        [Authorize]
        public async Task<UserPersonalResponseDto> GetUserPersonalInfo()
        {
            return await _usersService.GetUserPersonalInfo();
        }

        [HttpPut]
        [Authorize]
        public async Task<UserPersonalResponseDto> UpdateUser([FromBody] UserUpdatedRequestDto request)
        {
            return await _usersService.UpdateUser(request);
        }
    }
}

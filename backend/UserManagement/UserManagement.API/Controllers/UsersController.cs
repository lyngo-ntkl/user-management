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
        public async Task Register(UserRegistrationRequestDto request)
        {
            await _usersService.Register(request);
        }

        [HttpGet]
        public async Task<List<UserResponseDto>> GetUsers([FromQuery] string? userName)
        {
            return await _usersService.GetUsers(userName);
        }

        [HttpPost("login")]
        public async Task<AuthenticationResponseDto> Login(AuthenticationRequestDto request)
        {
            return await _usersService.Login(request);
        }
    }
}

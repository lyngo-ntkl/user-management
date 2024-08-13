using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Dtos.Responses;

namespace UserManagement.Application.Services
{
    public interface UsersService
    {
        public Task Register(UserRegistrationRequestDto request);
        public Task<List<UserResponseDto>> GetUsers(string? userName);
        public Task<AuthenticationResponseDto> Login(AuthenticationRequestDto request);
        public Task<UserResponseDto> GetUser(int id);
    }
}

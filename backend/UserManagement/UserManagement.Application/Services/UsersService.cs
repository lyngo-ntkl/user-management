using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Dtos.Responses;

namespace UserManagement.Application.Services
{
    public interface UsersService
    {
        public Task Register(UserRegistrationRequestDto request);
        public Task<List<UserResponseDto>> GetUsers(string? userName);
    }
}

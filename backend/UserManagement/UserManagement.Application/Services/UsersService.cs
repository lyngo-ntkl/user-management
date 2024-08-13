using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Dtos.Responses;

namespace UserManagement.Application.Services
{
    public interface UsersService
    {
        Task Register(UserRegistrationRequestDto request);
        Task<List<UserResponseDto>> GetUsers(string? userName);
        Task<AuthenticationResponseDto> Login(AuthenticationRequestDto request);
        Task<UserResponseDto> GetUser(int id);
        Task<UserPersonalResponseDto> GetUserPersonalInfo();
        Task<UserPersonalResponseDto> UpdateUser(UserUpdatedRequestDto request);
        Task DeleteUser();
    }
}

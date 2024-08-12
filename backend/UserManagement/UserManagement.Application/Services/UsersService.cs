using UserManagement.Application.Dtos.Requests;

namespace UserManagement.Application.Services
{
    public interface UsersService
    {
        public Task Register(UserRegistrationRequestDto request);
    }
}

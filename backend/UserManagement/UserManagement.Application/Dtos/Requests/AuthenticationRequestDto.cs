using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Dtos.Requests
{
    public class AuthenticationRequestDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

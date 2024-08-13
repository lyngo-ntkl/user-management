using System.ComponentModel.DataAnnotations;
using UserManagement.Application.Common;

namespace UserManagement.Application.Dtos.Requests
{
    public class UserUpdatedRequestDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        [Password]
        public string? Password { get; set; }
        public string? UserName { get; set; }
    }
}

using UserManagement.Domain.Enumerations;

namespace UserManagement.Domain.Entities
{
    public class User: BaseEntity
    {
        public required string Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required string UserName { get; set; }
        public required Role Role { get; set; }
    }
}

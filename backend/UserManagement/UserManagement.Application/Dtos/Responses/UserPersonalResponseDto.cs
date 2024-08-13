namespace UserManagement.Application.Dtos.Responses
{
    public class UserPersonalResponseDto
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
    }
}

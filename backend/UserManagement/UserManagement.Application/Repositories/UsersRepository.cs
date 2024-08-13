using UserManagement.Domain.Entities;

namespace UserManagement.Application.Repositories
{
    public interface UsersRepository: GenericRepository<User>
    {
        User? GetByEmail(string email);
        Task<User?> GetByEmailAsync(string email);
        bool ExistByEmail(string email);
    }
}

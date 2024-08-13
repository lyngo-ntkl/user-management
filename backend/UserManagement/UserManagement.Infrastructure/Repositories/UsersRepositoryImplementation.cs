using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Repositories;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.DatabaseContext;

namespace UserManagement.Infrastructure.Repositories
{
    public class UsersRepositoryImplementation : GenericRepositoryImplementation<User>, UsersRepository
    {
        public UsersRepositoryImplementation(ApplicationContext dbContext) : base(dbContext)
        {
        }

        public bool ExistByEmail(string email)
        {
            return _dbSet.Any(x => x.Email == email);
        }

        public User? GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(x => x.Email == email);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}

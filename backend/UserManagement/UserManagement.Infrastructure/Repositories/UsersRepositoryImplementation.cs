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
    }
}

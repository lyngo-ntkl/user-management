using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.DatabaseContext;

namespace UserManagement.Infrastructure.Repositories
{
    public interface UsersRepository: GenericRepository<User>
    {
    }

    public class UsersRepositoryImplementation : GenericRepositoryImplementation<User>, UsersRepository
    {
        public UsersRepositoryImplementation(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}

using UserManagement.Application.Repositories;
using UserManagement.Infrastructure.DatabaseContext;

namespace UserManagement.Infrastructure.Repositories
{
    public class UnitOfWorkImplementation : UnitOfWork
    {
        private readonly UsersRepository _usersRepository;
        private readonly ApplicationContext _applicationContext;

        public UnitOfWorkImplementation(UsersRepository usersRepository, ApplicationContext applicationContext)
        {
            _usersRepository = usersRepository;
            _applicationContext = applicationContext;
        }

        public UsersRepository UsersRepository => _usersRepository;

        public int Save()
        {
            return _applicationContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _applicationContext.SaveChangesAsync();
        }
    }
}

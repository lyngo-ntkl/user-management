using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Infrastructure.DatabaseContext;

namespace UserManagement.Infrastructure.Repositories
{
    public interface UnitOfWork
    {
        public int Save();
        public Task<int> SaveAsync();
        public UsersRepository UsersRepository { get; }
    }

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

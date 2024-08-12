namespace UserManagement.Application.Repositories
{
    public interface UnitOfWork
    {
        public int Save();
        public Task<int> SaveAsync();
        public UsersRepository UsersRepository { get; }
    }
}

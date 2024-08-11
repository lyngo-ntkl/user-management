using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Infrastructure.DatabaseContext;
using UserManagement.Infrastructure.Repositories;

namespace UserManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseMySQL(connectionString);
            });

            services.AddScoped<UnitOfWork, UnitOfWorkImplementation>();
            services.AddScoped<UsersRepository, UsersRepositoryImplementation>();
            return services;
        }
    }
}

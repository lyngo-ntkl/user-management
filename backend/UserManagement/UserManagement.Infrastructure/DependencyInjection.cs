using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Application.Repositories;
using UserManagement.Application.Services;
using UserManagement.Infrastructure.Configurations;
using UserManagement.Infrastructure.DatabaseContext;
using UserManagement.Infrastructure.Repositories;
using UserManagement.Infrastructure.Services;

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

            services.AddAutoMapper(config =>
            {
                config.AddProfile<UserManagementProfile>();
            });

            services.AddScoped<UnitOfWork, UnitOfWorkImplementation>();
            services.AddScoped<UsersRepository, UsersRepositoryImplementation>();

            services.AddScoped<UsersService, UsersServiceImplementation>();
            return services;
        }
    }
}

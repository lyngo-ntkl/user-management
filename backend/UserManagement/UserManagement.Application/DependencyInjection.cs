using Microsoft.Extensions.DependencyInjection;
using UserManagement.Application.Configurations;

namespace UserManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile<UserManagementProfile>();
            });
            return services;
        }
    }
}

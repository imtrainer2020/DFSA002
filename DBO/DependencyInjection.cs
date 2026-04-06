using DBO.IServices;
using DBO.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DBO
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectServices(this IServiceCollection services, string connectionString)
        {
            // Register the DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register your Services (Interface + Implementation)
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserDetailService, UserDetailService>();

            return services;
        }
    }
}

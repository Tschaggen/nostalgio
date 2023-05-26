using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Infrastructure.Persistence;

namespace StdsSocialMediaBackend.UserService.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterUserApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "UserDb");
            });

            return services;
        }
    }
}

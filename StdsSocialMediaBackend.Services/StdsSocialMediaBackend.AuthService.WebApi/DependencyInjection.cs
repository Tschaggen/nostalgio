using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StdsSocialMediaBackend.Infrastructure.Jwt;
using StdsSocialMediaBackend.Infrastructure.Persistence;

namespace StdsSocialMediaBackend.AuthService.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterAuthApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<JwtTokenHandler>();
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "AuthDb");
            });

            return services;
        }
    }
}

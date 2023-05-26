using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Infrastructure.Persistence;

namespace StdsSocialMediaBackend.MediaController.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterMediaApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PostDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "MediaDb");
            });

            return services;
        }
    }
}

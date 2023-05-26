using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Domain.Model.Auth;

namespace StdsSocialMediaBackend.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        public DbSet<AuthUser> Users { get; set; }
    }
}

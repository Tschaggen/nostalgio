using Microsoft.EntityFrameworkCore;
using StdsSocialMediaBackend.Domain.Model.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdsSocialMediaBackend.Infrastructure.Persistence
{
    public class PostDbContext : DbContext
    {
        public PostDbContext (DbContextOptions<PostDbContext> options) : base(options) 
        { 
        
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}

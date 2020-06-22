using Microsoft.EntityFrameworkCore;
using SmartTour.Domain;

namespace SmartTour.DataAccess
{
    public class PostsRepository : DbContext
    {
        public PostsRepository(DbContextOptions<PostsRepository> options) : base(options)
        {

        }

        public DbSet<PostEntity> Posts { get; set; }

    }
}

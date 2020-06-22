using Microsoft.EntityFrameworkCore;
using SmartTour.Domain;

namespace SmartTour.DataAccess
{
    public class UsersRepository:DbContext
    {
        public UsersRepository(DbContextOptions<UsersRepository> options):base(options)
        {

        }

        public DbSet<AuthEntity> Users { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using WebApiProject.Models;

namespace WebApiProject.Data
{
    public class GeoDbContext : DbContext
    {
        public GeoDbContext(DbContextOptions<GeoDbContext> options)
            : base(options)
        {

        }
        public DbSet<GeoMessage> GeoMessages { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

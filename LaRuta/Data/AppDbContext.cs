using Microsoft.EntityFrameworkCore;
using LaRuta.Model.Domain;

namespace LaRuta.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}

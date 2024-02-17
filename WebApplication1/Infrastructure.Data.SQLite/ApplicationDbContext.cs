using Microsoft.EntityFrameworkCore;
using Server.Domain;
using WebApplication1.Domain;

namespace WebApplication1
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options):
        base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }

    }
}

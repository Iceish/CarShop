using Microsoft.EntityFrameworkCore;
using Server.Domain;

namespace WebApplication1;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options):
    base(options){}

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleModel> VehicleModels { get; set; }
    public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }

}

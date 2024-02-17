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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleModel>().HasKey(x => x.Id);

            modelBuilder.Entity<Vehicle>().HasKey(x => x.Id);
            modelBuilder.Entity<Vehicle>()
                .HasOne<VehicleModel>()
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.VehicleModelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore;
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

            modelBuilder.Entity<Recipe>()
                .HasMany<Parameter>(x=>x.Parameters)
                .WithOne()
                .HasForeignKey(x=>x.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recipe>()
                .Property(x => x.RecipeName)
                .HasColumnName("Name");

            modelBuilder.Entity<Parameter>()
                .HasOne<Recipe>()
                .WithMany(x => x.Parameters)
                .HasForeignKey(x => x.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Parameter>() .HasKey(x => x.ID);


        }
    }
}

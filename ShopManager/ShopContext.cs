using Microsoft.EntityFrameworkCore;
using ShopManager.Entities;

namespace ShopManager
{
    public class ShopContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<SizeOption> SizeOptions { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialCount> MaterialCounts { get; set; }
        public DbSet<Color> Colors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shopmanager.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(i => i.SizeOptions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SizeOption>()
                .HasMany(so => so.MaterialCounts)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MaterialCount>()
                .HasOne(mc => mc.Material)
                .WithMany()
                .HasForeignKey(mc => mc.MaterialId);

            modelBuilder.Entity<Material>()
                .HasAlternateKey(m => m.FriendlyName)
                .HasName("AlternateKey_FriendlyName");
            modelBuilder.Entity<Material>()
                .HasMany(m => m.Colors)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
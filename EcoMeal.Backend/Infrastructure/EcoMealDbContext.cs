using Microsoft.EntityFrameworkCore;
using EcoMeal.Backend.Entities;

namespace EcoMeal.Backend.Infrastructure;

public class EcoMealDbContext : DbContext
{
    public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<BusinessType> BusinessTypes { get; set; }
    public DbSet<PackageType> PackageTypes { get; set; }
    public DbSet<Business> Business { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Business>()
        .HasOne(p => p.BusinessType)
        .WithMany(p => p.Businesses)
        .HasForeignKey(p => p.BusinessTypeId);
    }
}
using Microsoft.EntityFrameworkCore;
using EcoMeal.Backend.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EcoMeal.Backend.Infrastructure;

public class EcoMealDbContext : IdentityDbContext<User,IdentityRole<int>,int>
{
    public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options) : base(options)
    {
    }

    public DbSet<BusinessType> BusinessTypes { get; set; }
    public DbSet<PackageType> PackageTypes { get; set; }
    public DbSet<Business> Business { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Business>()
        .HasOne(p => p.BusinessType)
        .WithMany(p => p.Businesses)
        .HasForeignKey(p => p.BusinessTypeId);
    }
}
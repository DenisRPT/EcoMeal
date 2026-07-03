using Microsoft.EntityFrameworkCore;
using EcoMeal.Backend.Entities;

namespace EcoMeal.Backend.Infrastructure;

public class EcoMealDbContext : DbContext
{
    public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
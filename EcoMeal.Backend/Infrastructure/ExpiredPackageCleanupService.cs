using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace EcoMeal.Backend.Infrastructure;

public class ExpiredPackageCleanupService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ExpiredPackageCleanupService> _logger;

    public ExpiredPackageCleanupService(IServiceProvider services, ILogger<ExpiredPackageCleanupService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<EcoMealDbContext>();

                var now = DateTime.UtcNow;
                var expired = await db.Packages
                    .Where(p => p.PickupEnd <= now)
                    .ToListAsync(stoppingToken);

                if (expired.Any())
                {
                    db.Packages.RemoveRange(expired);
                    await db.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Removed {Count} expired packages", expired.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning expired packages");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}

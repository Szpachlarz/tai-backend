using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using tai_shop.Data;
using tai_shop.Enums;

namespace tai_shop.Services
{
    public class CartCleanupService : BackgroundService
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _services;

        public CartCleanupService(
            ApplicationDbContext context,
            IServiceProvider services)
        {
            _context = context;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<DbContext>();

                    var cleanupDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30));

                    var oldCarts = await _context.Carts
                        .Where(c => (c.Status == CartStatus.Converted ||
                                   (c.Status == CartStatus.Active && c.LastUpdated < cleanupDate)))
                        .ToListAsync(stoppingToken);

                    if (oldCarts.Any())
                    {
                        _context.Carts.RemoveRange(oldCarts);
                        await context.SaveChangesAsync(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
                catch (Exception ex)
                {
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
}

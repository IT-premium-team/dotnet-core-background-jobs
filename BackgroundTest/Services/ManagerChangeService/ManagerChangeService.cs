using BackgroundTest.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundTest.Services.ManagerChangeService
{
    public class ManagerChangeService : IManagerChangeService
    {
        private IServiceProvider _services { get; }

        private readonly ILogger<ManagerChangeService> _logger;

        public ManagerChangeService(
            IServiceProvider services,
            ILogger<ManagerChangeService> logger)
        {
            _services = services;

            _logger = logger;
        }

        public async Task ApplyChangesAsync()
        {
            using (var scope = _services.CreateScope())
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<MainDbContext>();

                var pendingChanges = await context.ManagerChanges
                    .Where(ch => !ch.IsChanged && ch.DateOfChange.Date <= DateTime.UtcNow.Date)
                    .ToListAsync();

                if (pendingChanges.Count() > 0)
                {
                    pendingChanges.All(ch => { ch.IsChanged = true; return true; });
                    context.ManagerChanges.UpdateRange(pendingChanges);
                    await context.SaveChangesAsync();

                    _logger.LogInformation("Changes are applied");
                }
            }
        }
    }
}

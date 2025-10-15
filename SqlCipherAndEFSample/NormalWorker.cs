using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SqlCipherAndEFCoreSample;

internal class NormalWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public NormalWorker(IServiceProvider serviceProvider, ILogger<NormalWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<EFInfrastructure.DbContext>();
        await db.Database.MigrateAsync(cancellationToken);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("sample work start");

                using var scope = _serviceProvider.CreateScope();
                using var db = scope.ServiceProvider.GetRequiredService<EFInfrastructure.DbContext>();
                db.Users.Add(new EFInfrastructure.DataModels.UserDataModel { Name = $"test_{DateTimeOffset.Now.ToString("HHmmss")}" });
                await db.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("sample work end(Users:{0})", db.Users.Count());

                await Task.Delay(3000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ExecuteAsync));
            }
        }
    }
}

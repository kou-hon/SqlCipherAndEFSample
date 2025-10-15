using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SqlCipherAndEFCoreSample;

internal class AbnormalWorker : BackgroundService
{
    private readonly ILogger _logger;

    public AbnormalWorker(IServiceProvider serviceProvider, ILogger<AbnormalWorker> logger)
    {
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000, stoppingToken);

        var okconnection = new SqliteConnectionStringBuilder
        {
            DataSource = "sample.db",
            Pooling = false,
            Password = "hoge",
        };
        //パスワードなしはアクセス失敗
        var ngconnection1 = new SqliteConnectionStringBuilder
        {
            DataSource = "sample.db",
            Pooling = false,
        };
        //パスワード違いはアクセス失敗
        var ngconnection2 = new SqliteConnectionStringBuilder
        {
            DataSource = "sample.db",
            Pooling = false,
            Password = "hogehoge",
        };

        IEnumerable<SqliteConnectionStringBuilder> list = [okconnection, ngconnection1, ngconnection2];

        foreach (var item in list.Select((x, n) => (x, n)))
        {
            try
            {
                var option = new DbContextOptionsBuilder<EFInfrastructure.DbContext>()
                    .UseSqlite(item.x.ConnectionString)
                    .Options;
                using var db = new EFInfrastructure.DbContext(option);

                _logger.LogInformation("Try: {0}, Users: {1}", item.n, await db.Users.CountAsync(stoppingToken));
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("Try: {0}, Canceled", item.n);
            }
            catch
            {
                _logger.LogError("Try: {0}, Failed", item.n);
            }
        }
    }
}

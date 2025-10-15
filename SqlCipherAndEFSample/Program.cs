using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlCipherAndEFCoreSample;

var connection = new SqliteConnectionStringBuilder
{
    DataSource = "sample.db",
    Pooling = false,
    Mode = SqliteOpenMode.ReadWriteCreate,
    Password = "hoge",
};

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddDbContext<EFInfrastructure.DbContext>(options => options.UseSqlite(connection.ConnectionString))
    .AddHostedService<NormalWorker>()
    .AddHostedService<AbnormalWorker>();

IHost host = builder.Build();
await host.RunAsync();

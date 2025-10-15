using EFInfrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;


namespace EFInfrastructure;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<UserDataModel> Users { get; set; }

    private readonly ILoggerFactory _loggerFactory = null!;

    public DbContext(DbContextOptions<DbContext> options, ILoggerFactory? loggerFactory = null) : base(options)
    {
        loggerFactory ??= NullLoggerFactory.Instance;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(_loggerFactory)
            .ConfigureWarnings(b => b.Log((RelationalEventId.CommandExecuted, LogLevel.Debug)));       //SQL実行ログをInformationからDebugに下げる（開発時以外は不要のため）

        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.AmbientTransactionWarning));
    }
}

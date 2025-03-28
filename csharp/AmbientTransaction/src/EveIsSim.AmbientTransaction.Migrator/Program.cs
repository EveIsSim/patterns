using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace EveIsSim.AmbientTransaction.Migrator;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = "Host=localhost;Port=54873;Database=eisambientts;Username=postgres;Password=postgres;";
        var serviceProvider = CreateServices(connectionString);

        using var scope = serviceProvider.CreateScope();
        UpdateDb(scope.ServiceProvider);

    }

    private static IServiceProvider CreateServices(string conString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(conString)
                    .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    private static void UpdateDb(IServiceProvider sp)
    {
        var runner = sp.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }

}

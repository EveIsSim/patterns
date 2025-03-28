using Dapper;

namespace EveIsSim.AmbientTransaction.App;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(conf =>
            {
                conf.UseStartup<Startup>();
            });
    }
}


using EveIsSim.AmbientTransaction.App.Controllers;
using EveIsSim.AmbientTransaction.App.Repositories;
using EveIsSim.AmbientTransaction.App.Services;
using EveIsSim.AmbientTransaction.Core;

public class Startup
{
    IConfiguration Configuration { get; }


    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

    }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddSingleton<IBankingService, BankingService>();
        services.AddSingleton<ITransactionScopeFactory, TransactionScopeFactory>();

        var conString = Configuration.GetConnectionString("DefaultConnection");
        services.AddSingleton<IBankingRepository>(p => new BankingRepository(conString!));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EveIsSim API");
            c.RoutePrefix = string.Empty;
        });

        app.UseRouting();
        app.UseEndpoints(e =>
        {
            e.MapBankEndpoints();
        });
    }
}

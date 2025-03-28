using Serilog;
using Microsoft.EntityFrameworkCore;
using CoBySi.Pomodoro.Repository.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using CoBySi.Pomodoro.Repository.settings;
namespace CoBySi.Pomodoro.Repository;

public static class RepositoryServiceExtension
{
    public async static Task<WebApplicationBuilder> AddCosmosDb(this WebApplicationBuilder builder)
    {
        var cosmosSettings = builder.Configuration.GetSection("cosmos").Get<CosmosUserSetting>();

        ArgumentNullException.ThrowIfNull(cosmosSettings);
        ArgumentException.ThrowIfNullOrEmpty(cosmosSettings?.ConnectionString);
        ArgumentException.ThrowIfNullOrEmpty(cosmosSettings?.DatabaseName);

        Log.Information("Adding db context {application}", nameof(PomodoroAuth));
        builder.Services.AddDbContextFactory<PomodoroAuth>(
                    options => options.UseCosmos(
                        connectionString: cosmosSettings.ConnectionString,
                        databaseName: cosmosSettings.DatabaseName,
                                options =>
            {
                // options.ConnectionMode(ConnectionMode.);
                // options.LimitToEndpoint();
                // options.GatewayModeMaxConnectionLimit(32);
                // options.RequestTimeout(TimeSpan.FromMinutes(1));

            })
        );
        if (cosmosSettings.Setup)
        {
            Log.Information("Creating comsoms db {dbName}", cosmosSettings.DatabaseName);
            var builder1 = new DbContextOptionsBuilder<PomodoroAuth>();
            builder1.UseCosmos(cosmosSettings.ConnectionString, cosmosSettings.DatabaseName);

            using var dbContext = new PomodoroAuth(builder1.Options);
            await dbContext.Database.EnsureCreatedAsync();
        }
        return builder;
    }

}

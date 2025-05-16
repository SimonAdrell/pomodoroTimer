using Serilog;
using Microsoft.EntityFrameworkCore;
using CoBySi.Pomodoro.Repository.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Azure.Cosmos;
namespace CoBySi.Pomodoro.Repository;

public static class RepositoryServiceExtension
{
    public async static Task<WebApplicationBuilder> AddCosmosDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("cosmosdb")
            ?? throw new InvalidOperationException("CosmosDb connection string not found in configuration.");

        var cosmosSettings = builder.Configuration.GetSection("UserDbSettings").Get<CosmosUserSetting>();

        ArgumentException.ThrowIfNullOrEmpty(cosmosSettings?.DatabaseName);

        Log.Information("Adding db context {Application}", nameof(PomodoroAuth));
        builder.Services.AddDbContextFactory<PomodoroAuth>(
                    options => options.UseCosmos(
                        connectionString: connectionString,
                        databaseName: cosmosSettings.DatabaseName,
                                options =>
            {
            })
        );
        if (cosmosSettings.Setup)
        {
            Log.Information("Creating comsoms db {DbName}", cosmosSettings.DatabaseName);
            var dbContextBuilder = new DbContextOptionsBuilder<PomodoroAuth>();
            dbContextBuilder.UseCosmos(connectionString, cosmosSettings.DatabaseName, options =>
            {
                options.ConnectionMode(ConnectionMode.Direct);
            });
            using var dbContext = new PomodoroAuth(dbContextBuilder.Options);
            await dbContext.Database.EnsureCreatedAsync();
        }
        return builder;
    }

}

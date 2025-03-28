using CoBySi.Pomodoro.Repository.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoBySi.Pomodoro.Repository;

public class PomdoroDbContextFactory : IDesignTimeDbContextFactory<PomodoroAuth>
{
    public PomdoroDbContextFactory()
    {
    }

    public PomodoroAuth CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PomodoroAuth>();
        optionsBuilder
            .UseCosmos(
                connectionString: "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                databaseName: "ApplicationDB",
                cosmosOptionsAction: options =>
                {
                    options.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct);
                    options.MaxRequestsPerTcpConnection(16);
                    options.MaxTcpConnectionsPerEndpoint(32);
                });

        return new PomodoroAuth(optionsBuilder.Options);
    }
}

using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CoBySi.Pomodoro.Repository.Repositories;

public abstract class CosmosRepositoryBase<TEntity>
    where TEntity : UserBaseEntity
{
    private readonly CosmosBaseSettings _settings;
    private readonly string _connectionString;

    protected CosmosRepositoryBase(CosmosBaseSettings settings, IConfiguration configuration)
    {
        _settings = settings;
        _connectionString = configuration.GetConnectionString("cosmosdb") ??
                          throw new InvalidOperationException("CosmosDb connection string not found in configuration.");
    }

    public async Task<TEntity> Upsert(TEntity entity, CancellationToken cancellationToken)
    {
        Container container = await CosmosHelper.CreateIfNotExist(_settings, _connectionString);
        var response = await container.UpsertItemAsync(entity,
            new PartitionKey(entity.UserId), cancellationToken: cancellationToken);

        return response.Resource;
    }

    public async Task<TEntity?> GetFirstOrDefaultByUserId(string userId, CancellationToken cancellationToken)
    {
        var container = await CosmosHelper.CreateIfNotExist(_settings, _connectionString);
        var query = new QueryDefinition($"SELECT * FROM c WHERE c.userId = @userId")
            .WithParameter("@userId", userId);
        var iterator = container.GetItemQueryIterator<TEntity>(query);
        var notification = await iterator.ReadNextAsync(cancellationToken);
        return notification.FirstOrDefault();
    }
}

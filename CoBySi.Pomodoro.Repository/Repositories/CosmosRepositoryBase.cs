using CoBySi.Pomodoro.Repository.Models;
using Microsoft.Azure.Cosmos;

namespace CoBySi.Pomodoro.Repository.Repositories;

public abstract class CosmosRepositoryBase<TEntity>
    where TEntity : UserBaseEntity
{
    private readonly CosmosBaseSettings _settings;

    protected CosmosRepositoryBase(CosmosBaseSettings settings)
    {
        _settings = settings;
    }

    public async Task<TEntity> Upsert(TEntity entity, CancellationToken cancellationToken)
    {
        Container container = await CosmosHelper.CreateIfNotExist(_settings);
        var response = await container.UpsertItemAsync(entity,
            new PartitionKey(entity.UserId), cancellationToken: cancellationToken);

        return response.Resource;
    }

    public async Task<TEntity?> GetFirstOrDefaultByUserId(string userId, CancellationToken cancellationToken)
    {
        var container = await CosmosHelper.CreateIfNotExist(_settings);
        var query = new QueryDefinition($"SELECT * FROM c WHERE c.UserId = @userId")
            .WithParameter("@userId", userId);
        var iterator = container.GetItemQueryIterator<TEntity>(query);
        var notification = await iterator.ReadNextAsync(cancellationToken);
        return notification.FirstOrDefault();
    }
}

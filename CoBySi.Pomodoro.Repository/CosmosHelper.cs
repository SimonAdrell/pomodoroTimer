using Azure.Identity;
using Microsoft.Azure.Cosmos;

namespace CoBySi.Pomodoro.Repository;

public class CosmosHelper
{
    public static async Task<Container> CreateIfNotExist(CosmosBaseSettings settings)
    {
        var client = new CosmosClient(
            connectionString: settings.ConnectionString,
    new CosmosClientOptions
    {
        SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase },
    }
        );
        await client.CreateDatabaseIfNotExistsAsync(settings.DatatabaseName);
        var database = client.GetDatabase(settings.DatatabaseName);
        await database.CreateContainerIfNotExistsAsync(id: settings.ContainerName, partitionKeyPath: "/userId");
        return database.GetContainer(settings.ContainerName);
    }
}

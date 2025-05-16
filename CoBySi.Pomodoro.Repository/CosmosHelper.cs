using Azure.Identity;
using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Azure.Cosmos;

namespace CoBySi.Pomodoro.Repository;

public static class CosmosHelper
{
    public static async Task<Container> CreateIfNotExist(CosmosBaseSettings settings, string connectionString)
    {
        var client = new CosmosClient(connectionString,
            new DefaultAzureCredential(),
           new CosmosClientOptions
           {
               SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase },
           });

#if DEBUG
        client = new CosmosClient(
        connectionString: connectionString,
        new CosmosClientOptions
        {
            SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase },
        });
#endif

        await client.CreateDatabaseIfNotExistsAsync(settings.DatabaseName);
        var database = client.GetDatabase(settings.DatabaseName);
        await database.CreateContainerIfNotExistsAsync(id: settings.ContainerName, partitionKeyPath: "/userId");
        return database.GetContainer(settings.ContainerName);
    }
}

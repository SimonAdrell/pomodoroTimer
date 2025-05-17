#pragma warning disable ASPIRECOSMOSDB001
var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisInsight();

var cosmosdb = builder.AddAzureCosmosDB("cosmosdb")
            .RunAsPreviewEmulator(
                     emulator =>
                     {
                         emulator.WithDataExplorer();
                         emulator.WithDataVolume();
                         emulator.WithGatewayPort(7777);
                     });

builder
    .AddProject<Projects.CoBySi_Pomodoro_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(cosmosdb)
    .WaitFor(cosmosdb);

await builder.Build().RunAsync();

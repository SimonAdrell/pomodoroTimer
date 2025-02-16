using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CoBySi.Pomodoro.Repository.Repositories;

public class PomodorosRepository : IPomodorosRepository
{
    private readonly MongoSettiings _settings;

    public PomodorosRepository(IOptions<MongoSettiings> mongoSettingsOptions)
    {
        _settings = mongoSettingsOptions.Value;
    }

    public async Task<PomodoroEntity?> SavePomodoroItemAsync(PomodoroEntity pomodoroItem, CancellationToken cancellationToken)
    {
        var client = new MongoClient(_settings.ConnectionString);
        var collection = client.GetDatabase(_settings.Database)
            .GetCollection<PomodoroEntity>(_settings.Collections?.PomodoroCollection);
        var filter = Builders<PomodoroEntity>.Filter.Eq(x => x.Id, pomodoroItem.Id);

        var result = await collection.UpdateOneAsync(filter, new UpdateDefinitionBuilder<PomodoroEntity>()
            .Set(x => x.Started, pomodoroItem.Started)
            .Set(x => x.ExpectedToEnd, pomodoroItem.ExpectedToEnd)
            .Set(x => x.Ended, pomodoroItem.Ended)
            .Set(x => x.TotalNumberOfSeconds, pomodoroItem.TotalNumberOfSeconds)
            .Set(x => x.UserId, pomodoroItem.UserId)
            .Set(x => x.SessionID, pomodoroItem.SessionID)
            .Set(x => x.PomodoroNumber, pomodoroItem.PomodoroNumber),
            new UpdateOptions { IsUpsert = true }, cancellationToken);

        if (result.IsAcknowledged)
            pomodoroItem.Id = result.UpsertedId.AsString;
        return result.IsAcknowledged ? pomodoroItem : null;
    }
}

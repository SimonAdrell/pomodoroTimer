using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.Repositories;
using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CoBySi.Pomodoro.Repository;

public class UserSettingsRepository : IUserSettingsRepository
{
    private readonly MongoSettiings _mongoSettiings;
    public UserSettingsRepository(IOptions<MongoSettiings> mongoSettiings)
    {
        _mongoSettiings = mongoSettiings.Value;
    }

    public async Task<UserPomodoroSettingsEntity?> GetUserPomodoroSettingsAsync(string userId, CancellationToken cancellationToken)
    {
        var client = new MongoClient(_mongoSettiings.ConnectionString);
        var collection = client.GetDatabase(_mongoSettiings.Database).GetCollection<UserPomodoroSettingsEntity>(_mongoSettiings.Collections?.UserSettingsCollection);
        var filter = Builders<UserPomodoroSettingsEntity>.Filter.Eq(x => x.UserID, userId);
        var setting = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return setting;
    }

    public async Task SavePomodoroSettingsAsync(UserPomodoroSettingsEntity entity, CancellationToken cancellationToken)
    {
        var client = new MongoClient(_mongoSettiings.ConnectionString);
        var collection = client.GetDatabase(_mongoSettiings.Database)
            .GetCollection<UserPomodoroSettingsEntity>(_mongoSettiings.Collections?.UserSettingsCollection);
        var filter = Builders<UserPomodoroSettingsEntity>.Filter.Eq(x => x.UserID, entity.UserID);

        await collection.UpdateOneAsync(filter, new UpdateDefinitionBuilder<UserPomodoroSettingsEntity>()
            .Set(x => x.MinutesPerPomodoro, entity.MinutesPerPomodoro)
            .Set(x => x.MinutesPerShortBreak, entity.MinutesPerShortBreak)
            .Set(x => x.MinutesPerLongBreak, entity.MinutesPerLongBreak)
            .Set(x => x.PomodorosBeforeLongBreak, entity.PomodorosBeforeLongBreak),
            new UpdateOptions { IsUpsert = true }, cancellationToken);
    }

    public async Task SaveNotificationItemAsync(NotificationEntity notificationItem, CancellationToken cancellationToken)
    {
        var client = new MongoClient(_mongoSettiings.ConnectionString);
        var collection = client.GetDatabase(_mongoSettiings.Database)
            .GetCollection<NotificationEntity>(_mongoSettiings.Collections?.NotificationSettingsCollection);
        var filter = Builders<NotificationEntity>.Filter.Eq(x => x.UserId, notificationItem.UserId);

        await collection.UpdateOneAsync(filter, new UpdateDefinitionBuilder<NotificationEntity>()
           .Set(x => x.LastChanged, notificationItem.LastChanged)
           .Set(x => x.UserId, notificationItem.UserId)
           .Set(x => x.NotificationEnabled, notificationItem.NotificationEnabled)
           .Set(x => x.NotificationSoundEnabled, notificationItem.NotificationSoundEnabled)
           .Set(x => x.SoundID, notificationItem.SoundID),
           new UpdateOptions { IsUpsert = true }, cancellationToken);
    }

    public async Task<NotificationEntity?> GetUserNotificationSettingsAsync(string userId, CancellationToken cancellationToken)
    {
        var client = new MongoClient(_mongoSettiings.ConnectionString);
        var collection = client.GetDatabase(_mongoSettiings.Database)
            .GetCollection<NotificationEntity>(_mongoSettiings.Collections?.NotificationSettingsCollection);
        var filter = Builders<NotificationEntity>.Filter.Eq(x => x.UserId, userId);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}

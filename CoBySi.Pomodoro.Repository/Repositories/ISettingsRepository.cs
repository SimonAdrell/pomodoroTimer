using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Repository.Repositories;

public interface ISettingsRepository
{
    public Task<UserSettings?> UpsertUserSetting(UserSettings userSettings, CancellationToken cancellationToken);
    public Task<UserSettings?> GetUserSettingsByUserId(string userId, CancellationToken cancellationToken);
}

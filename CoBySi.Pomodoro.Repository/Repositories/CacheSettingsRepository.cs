using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Web.Cache;

namespace CoBySi.Pomodoro.Repository.Repositories;

public class CacheSettingsRepository : ISettingsRepository
{
    private readonly ISettingsRepository _settingsRepository;
    private readonly ISettingsCache _settingsCache;

    public CacheSettingsRepository(ISettingsRepository settingsRepository, ISettingsCache settingsCache)
    {
        _settingsRepository = settingsRepository;
        _settingsCache = settingsCache;
    }

    public async Task<UserSettings?> GetUserSettingsByUserId(string userId, CancellationToken cancellationToken)
    {
        var userSettings = await _settingsCache.GetAsync(userId, cancellationToken);
        if (userSettings is not null)
            return userSettings;

        userSettings = await _settingsRepository.GetUserSettingsByUserId(userId, cancellationToken);
        if (userSettings is not null)
            await _settingsCache.SetAsync(userId, userSettings, cancellationToken);
        return userSettings;
    }

    public async Task<UserSettings?> UpsertUserSetting(UserSettings userSettings, CancellationToken cancellationToken)
    {
        var settings = await _settingsRepository.UpsertUserSetting(userSettings, cancellationToken);
        if (settings is not null)
            await _settingsCache.SetAsync(userSettings.UserId, settings, cancellationToken);

        return settings;
    }
}

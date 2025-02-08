using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Repository.Repositories;
using CoBySi.Pomodoro.Web.Cache;
using CoBySi.Pomodoro.Web.Converter;
using Microsoft.Extensions.Options;

namespace CoBySi.Pomodoro.Web.Services;

public class PomodoroSettingsService : IPomodoroSettingsService
{
    private readonly PomodoroSettings _pomodoroSettings;
    private readonly ISettingsCache _settingsCache;
    private readonly IUserSettingsRepository _userSettingsRepository;
    public PomodoroSettingsService(IOptions<PomodoroSettings> pomodoroSettings, IUserSettingsRepository userSettingsRepository, ISettingsCache settingsCache)
    {
        _pomodoroSettings = pomodoroSettings.Value;
        _userSettingsRepository = userSettingsRepository;
        _settingsCache = settingsCache;
    }

    public async Task<PomodoroSettings> GetUserPomodoroSettingsAsync(string userId, string? sessionid, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(userId))
            return await GetUserSettings(userId, cancellationToken);

        if (!string.IsNullOrEmpty(sessionid))
            return await GetSettingsBySessionID(sessionid, cancellationToken);

        return _pomodoroSettings;
    }

    private async Task<PomodoroSettings> GetUserSettings(string userId, CancellationToken cancellationToken)
    {
        var cachedSettings = await _settingsCache.GetAsync<PomodoroSettings>($"user:{userId}", cancellationToken);
        if (cachedSettings != null)
            return cachedSettings;

        var userSettings = await _userSettingsRepository.GetUserPomodoroSettingsAsync(userId, cancellationToken);
        var settings = userSettings?.ConvertToPomodoroSettings() ?? _pomodoroSettings;
        await _settingsCache.SetAsync($"user:{userId}", settings, cancellationToken);
        return settings;
    }

    private async Task<PomodoroSettings> GetSettingsBySessionID(string sessionid, CancellationToken cancellationToken)
    {
        var cachedSettings = await _settingsCache.GetAsync<PomodoroSettings>($"session:{sessionid}", cancellationToken);
        if (cachedSettings != null)
            return cachedSettings;

        await _settingsCache.SetAsync($"session:{sessionid}", _pomodoroSettings, cancellationToken);
        return _pomodoroSettings;
    }

    public async Task SavePomodoroSettingsAsync(string userId, string? sessionid, PomodoroSettings entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!string.IsNullOrEmpty(userId))
        {
            await _userSettingsRepository.SavePomodoroSettingsAsync(entity.ConvertToUserPomodoroSettingsEntity(userId), cancellationToken);
            await _settingsCache.SetAsync($"user:{userId}", entity, cancellationToken);
        }
        if (!string.IsNullOrEmpty(sessionid))
            await _settingsCache.SetAsync($"session:{sessionid}", entity, cancellationToken);

    }
}

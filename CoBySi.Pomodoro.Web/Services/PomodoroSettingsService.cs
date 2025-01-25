using CoBySi.Pomodoro.Repository;
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

    public async Task<PomodoroSettings> GetUserPomodoroSettingsAsync(string userId)
    {
        var cachedSettings = _settingsCache.Get<PomodoroSettings>(userId);
        if (cachedSettings != null)
            return cachedSettings;

        if (string.IsNullOrEmpty(userId))
            return _pomodoroSettings;

        var userSettings = await _userSettingsRepository.GetUserPomodoroSettingsAsync(userId);
        var settings = userSettings != null ? userSettings.ConvertToPomodoroSettings() : _pomodoroSettings;
        _settingsCache.Set(userId, settings);
        return settings;
    }

    public async Task SavePomodoroSettings(string userId, PomodoroSettings entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!string.IsNullOrEmpty(userId))
        {
            await _userSettingsRepository.SavePomodoroSettings(entity.ConvertToUserPomodoroSettingsEntity(userId));
            _settingsCache.Set(userId, entity);
        }
    }
}

using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.Repositories;
using CoBySi.Pomodoro.Repository.Repositories.Cache;
using CoBySi.Pomodoro.Web.Converter;
using Microsoft.Extensions.Options;

namespace CoBySi.Pomodoro.Web.Services;

public class SettingsService : ISettingsService
{
    private readonly PomodoroSettings _pomodoroSettings;
    private readonly ISettingsRepository _settingsRepository;
    public SettingsService(IOptions<PomodoroSettings> pomodoroSettings, ISettingsRepository userSettingsRepository, ISettingsCache settingsCache)
    {
        ArgumentNullException.ThrowIfNull(pomodoroSettings.Value);
        _pomodoroSettings = pomodoroSettings.Value;
        _settingsRepository = userSettingsRepository;
    }

    public async Task<PomodoroSettings> GetUserPomodoroSettingsAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        return await GetUserSettings(userId, cancellationToken) ?? _pomodoroSettings;
    }

    private async Task<PomodoroSettings?> GetUserSettings(string userId, CancellationToken cancellationToken)
    {
        var userSettings = await _settingsRepository.GetUserSettingsByUserId(userId, cancellationToken);
        return userSettings?.PomodoroSettings.ConvertToPomodoroSettings();
    }

    public async Task<NotificationEntity?> GetNotificationSettingsAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        var userSettings = await _settingsRepository.GetUserSettingsByUserId(userId, cancellationToken);
        return userSettings?.NotificationEntity;
    }

    public async Task<UserSettings?> GetUserSettingsAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        return await _settingsRepository.GetUserSettingsByUserId(userId, cancellationToken);
    }

    public async Task SaveSettings(string userId, UserSettings entity, CancellationToken cancellationToken)
    {
        await _settingsRepository.UpsertUserSetting(entity, cancellationToken);
    }
}

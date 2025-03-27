using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.Repositories;
using CoBySi.Pomodoro.Web.Cache;
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
        return userSettings?.PommodoroSettings.ConvertToPomodoroSettings();
    }

    public async Task SavePomodoroSettingsAsync(string userId, PomodoroSettings entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

        var setting = await _settingsRepository.GetUserSettingsByUserId(userId, cancellationToken);
        if (setting is null)
        {
            await _settingsRepository.UpsertUserSetting(new UserSettings(userId)
            {
                NotificationEntity = null,
                PommodoroSettings = entity.ConvertToUserPomodoroSettingsEntity()
            }, cancellationToken);
            return;
        }

        setting.PommodoroSettings = entity.ConvertToUserPomodoroSettingsEntity();
        await _settingsRepository.UpsertUserSetting(setting, cancellationToken);
    }

    public async Task SaveNotificationSettingsAsync(string userId, NotificationEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

        var setting = await _settingsRepository.GetUserSettingsByUserId(userId, cancellationToken);
        if (setting is null)
        {
            await _settingsRepository.UpsertUserSetting(new UserSettings(userId)
            {
                NotificationEntity = entity,
                PommodoroSettings = null,
            }, cancellationToken);
            return;
        }

        setting.NotificationEntity = entity;
        await _settingsRepository.UpsertUserSetting(setting, cancellationToken);
    }

    public async Task<NotificationEntity?> GetNotificationSettingsAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        var userSettings = await _settingsRepository.GetUserSettingsByUserId(userId, cancellationToken);
        return userSettings?.NotificationEntity;
    }
}

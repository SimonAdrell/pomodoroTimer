using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Web.Converter;
using Microsoft.Extensions.Options;

namespace CoBySi.Pomodoro.Web.Services;

public class PomodoroSettingsService : IPomodoroSettingsService
{
    private readonly PomodoroSettings _pomodoroSettings;
    private readonly IUserSettingsRepository _userSettingsRepository;
    public PomodoroSettingsService(IOptions<PomodoroSettings> pomodoroSettings, IUserSettingsRepository userSettingsRepository)
    {
        _pomodoroSettings = pomodoroSettings.Value;
        _userSettingsRepository = userSettingsRepository;
    }

    public async Task<PomodoroSettings> GetUserPomodoroSettingsAsync(string userId)

    {
        // Check user redis cache
        if (string.IsNullOrEmpty(userId))
            return _pomodoroSettings;

        var userSettings = await _userSettingsRepository.GetUserPomodoroSettingsAsync(userId);
        return userSettings != null ? userSettings.ConvertToPomodoroSettings() : _pomodoroSettings;
    }

    public async Task SavePomodoroSettings(string userId, PomodoroSettings entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!string.IsNullOrEmpty(userId))
            await _userSettingsRepository.SavePomodoroSettings(entity.ConvertToUserPomodoroSettingsEntity(userId));
    }
}

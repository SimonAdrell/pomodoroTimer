using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Web.Converter;

namespace CoBySi.Pomodoro.Web.PomodoroProperties;

public class PomdoroPropertiesHandler : IPomdoroPropertiesHandler
{
    private readonly PomodoroSettings _pomodoroSettings;
    private readonly IUserSettingsRepository _userSettingsRepository;

    public PomdoroPropertiesHandler(PomodoroSettings pomodoroSettings, IUserSettingsRepository userSettingsRepository)
    {
        _pomodoroSettings = pomodoroSettings;
        _userSettingsRepository = userSettingsRepository;
    }

    public async Task<PomodoroSettings> GetPomodoroPropertiesAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            return _pomodoroSettings;

        var userSettings = await _userSettingsRepository.GetUserPomodoroSettingsAsync(userId);
        if (userSettings != null)
            return userSettings.ConvertToPomodoroSettings();

        return _pomodoroSettings;
    }
}

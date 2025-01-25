namespace CoBySi.Pomodoro.Web.Services;

public interface IPomodoroSettingsService
{
    Task<PomodoroSettings> GetUserPomodoroSettingsAsync(string userId);
    Task SavePomodoroSettings(string userId, PomodoroSettings entity);
}

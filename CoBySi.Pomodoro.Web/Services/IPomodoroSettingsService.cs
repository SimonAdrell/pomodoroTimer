namespace CoBySi.Pomodoro.Web.Services;

public interface IPomodoroSettingsService
{
    Task<PomodoroSettings> GetUserPomodoroSettingsAsync(string userId, string? sessionid, CancellationToken cancellationToken);
    Task SavePomodoroSettingsAsync(string userId, string? sessionid, PomodoroSettings entity, CancellationToken cancellationToken);
}

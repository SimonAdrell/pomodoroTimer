using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Web.Services;

public interface ISettingsService
{
    Task<PomodoroSettings> GetUserPomodoroSettingsAsync(string userId, CancellationToken cancellationToken);
    Task<NotificationEntity?> GetNotificationSettingsAsync(string userId, CancellationToken cancellationToken);
    Task<UserSettings?> GetUserSettingsAsync(string userId, CancellationToken cancellationToken);
    Task SaveSettings(string userId, UserSettings entity, CancellationToken cancellationToken);

}

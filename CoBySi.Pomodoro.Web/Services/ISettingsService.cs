using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Web.Services;

public interface ISettingsService
{
    Task<PomodoroSettings> GetUserPomodoroSettingsAsync(string userId, CancellationToken cancellationToken);
    Task SavePomodoroSettingsAsync(string userId, PomodoroSettings entity, CancellationToken cancellationToken);
    Task SaveNotificationSettingsAsync(string userId, NotificationEntity entity, CancellationToken cancellationToken);
    Task<NotificationEntity?> GetNotificationSettingsAsync(string userId, CancellationToken cancellationToken);
    Task<UserSettings?> GetUserSettingsAsync(string userId, CancellationToken cancellationToken);

}

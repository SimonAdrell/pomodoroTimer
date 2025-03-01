using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Repository.Repositories;

public interface IUserSettingsRepository
{
    Task<UserPomodoroSettingsEntity?> GetUserPomodoroSettingsAsync(string userId, CancellationToken cancellationToken);
    Task SavePomodoroSettingsAsync(UserPomodoroSettingsEntity entity, CancellationToken cancellationToken);
    Task<NotificationEntity?> GetUserNotificationSettingsAsync(string userId, CancellationToken cancellationToken);
    Task SaveNotificationItemAsync(NotificationEntity notificationItem, CancellationToken cancellationToken);
}

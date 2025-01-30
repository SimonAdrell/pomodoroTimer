using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Repository;

public interface IUserSettingsRepository
{
    Task<UserPomodoroSettingsEntity?> GetUserPomodoroSettingsAsync(string userId, CancellationToken cancellationToken);
    Task SavePomodoroSettingsAsync(UserPomodoroSettingsEntity entity, CancellationToken cancellationToken);

}

using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Repository;

public interface IUserSettingsRepository
{
    Task<UserPomodoroSettingsEntity?> GetUserPomodoroSettingsAsync(string userId);
    Task SavePomodoroSettings(UserPomodoroSettingsEntity entity);

}

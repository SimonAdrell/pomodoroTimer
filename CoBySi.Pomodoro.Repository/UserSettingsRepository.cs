using System;
using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Repository;

public class UserSettingsRepository : IUserSettingsRepository
{
    public Task<UserPomodoroSettingsEntity?> GetUserPomodoroSettingsAsync(Guid userId)
    {
        return Task.FromResult<UserPomodoroSettingsEntity?>(null);
    }
}

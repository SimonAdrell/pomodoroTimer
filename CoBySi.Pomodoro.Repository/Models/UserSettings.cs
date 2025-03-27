namespace CoBySi.Pomodoro.Repository.Models;

public record UserSettings(string UserId) : UserBaseEntity(UserId)
{
    public required PomodoroSettingsEntity? PommodoroSettings { get; set; }
    public required NotificationEntity? NotificationEntity { get; set; }

}


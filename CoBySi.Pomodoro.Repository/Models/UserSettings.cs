using Newtonsoft.Json;

namespace CoBySi.Pomodoro.Repository.Models;

public record UserSettings(string UserId) : UserBaseEntity(UserId)
{
    [JsonProperty("id")]
    public string id { get; set; } = UserId;
    public required PomodoroSettingsEntity? PomodoroSettings { get; set; }
    public required NotificationEntity? NotificationEntity { get; set; }

}


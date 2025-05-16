namespace CoBySi.Pomodoro.Repository.settings;

public class RedisSettings
{
    public string? InstanceName { get; set; }
    public CacheSetting? TimerCache { get; set; }
    public CacheSetting? SettingsCache { get; set; }
}

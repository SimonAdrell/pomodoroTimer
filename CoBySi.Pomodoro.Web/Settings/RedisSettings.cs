namespace CoBySi.Pomodoro.Web.Settings;

public class RedisSettings
{
    public string? ConnectionString { get; set; }
    public string? InstanceName { get; set; }
    public CacheSetting? PomodoroCache { get; set; }
}

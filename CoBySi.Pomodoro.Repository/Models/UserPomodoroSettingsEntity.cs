namespace CoBySi.Pomodoro.Repository.Models;

public class UserPomodoroSettingsEntity
{
    public int MinutesPerPomodoro { get; set; }
    public int MinutesPerShortBreak { get; set; }
    public int MinutesPerLongBreak { get; set; }
    public int PomodorosBeforeLongBreak { get; set; }
}

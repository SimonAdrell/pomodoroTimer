namespace CoBySi.Pomodoro.Repository.Models;

public class PomodoroSettingsEntity
{
    public double MinutesPerPomodoro { get; set; }
    public double MinutesPerShortBreak { get; set; }
    public double MinutesPerLongBreak { get; set; }
    public double PomodorosBeforeLongBreak { get; set; }
}

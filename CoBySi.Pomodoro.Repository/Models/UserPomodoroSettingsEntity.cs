namespace CoBySi.Pomodoro.Repository.Models;

public class PomodoroSettingsEntity
{
    public string? Id { get; set; }
    public double MinutesPerPomodoro { get; set; }
    public double MinutesPerShortBreak { get; set; }
    public double MinutesPerLongBreak { get; set; }
    public double PomodorosBeforeLongBreak { get; set; }
}

namespace CoBySi.Pomodoro.Timer;

public class TimeChangedEventArgs : EventArgs
{
    public double? NumberOfSeconds { get; set; }
    public PomodoroState State { get; set; }
}

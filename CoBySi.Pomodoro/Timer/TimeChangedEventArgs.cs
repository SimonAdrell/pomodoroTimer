namespace CoBySi.Pomodoro.Timer;

public class TimeChangedEventArgs : EventArgs
{
    public double? NumberOfSecondsElapsed { get; set; }
    public PomodoroState State { get; set; }
}

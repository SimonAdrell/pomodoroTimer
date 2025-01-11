namespace CoBySi.Pomodoro.Timer;

public class TimeChangedEventArgs : EventArgs
{
    public double? NumberOfSecondsLeft { get; set; }
    public PomodoroState State { get; set; }
}

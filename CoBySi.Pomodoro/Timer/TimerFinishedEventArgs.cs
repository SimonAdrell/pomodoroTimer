namespace CoBySi.Pomodoro.Timer;

public class TimerFinishedEventArgs : EventArgs
{
    public PomodoroState StateFinished { get; set; }
}

using CoBySi.Pomodoro.Models;

namespace CoBySi.Pomodoro.Timer;

public class TimerChangedEventArgs : EventArgs
{
    public double? NumberOfSecondsLeft { get; set; }
    public PomodoroItem? Item { get; set; }
    public TimerEventType EventType { get; set; }
}

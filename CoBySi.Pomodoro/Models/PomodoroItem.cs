using CoBySi.Pomodoro.Timer;

namespace CoBySi.Pomodoro.Models;

public class PomodoroItem
{
    public Guid Id { get; set; }
    public PomodoroStatus Status { get; set; }
    public PomodoroStatus NextStatus { get; set; }
    public double TotalNumberOfSeconds { get; set; }
    public string? UserId { get; set; }
    public string? SessionID { get; set; }
    public int NumberOfPomodoros { get; set; }
}

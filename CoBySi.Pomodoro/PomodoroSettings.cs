using System;

namespace CoBySi.Pomodoro;

public class PomodoroSettings
{
    public int MinutesPerPomodoro { get; set; }
    public int MinutesPerShortBreak { get; set; }
    public int MinutesPerLongBreak { get; set; }
    public int PomodorosBeforeLongBreak { get; set; }
}

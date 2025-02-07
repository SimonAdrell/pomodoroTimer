using CoBySi.Pomodoro.Models;
using CoBySi.Pomodoro.Timer;

namespace CoBySi.Pomodoro;

public interface IPomodorHandler
{
    event EventHandler<TimerChangedEventArgs>? TimerChanged;
    void StartNext(PomodoroSettings? pomodoroSettings);
    void Stop();
    bool IsRunning();
}

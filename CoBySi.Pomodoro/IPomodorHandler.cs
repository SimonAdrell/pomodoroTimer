using CoBySi.Pomodoro.Timer;

namespace CoBySi.Pomodoro;

public interface IPomodorHandler
{
    event AsyncEventHandler<TimerChangedEventArgs>? TimerChangedAsync;
    void StartNext(PomodoroSettings? pomodoroSettings);
    void Stop();
    bool IsRunning();
}

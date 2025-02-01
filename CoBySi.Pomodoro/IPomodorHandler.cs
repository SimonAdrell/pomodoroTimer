using CoBySi.Pomodoro.Timer;

namespace CoBySi.Pomodoro;

public interface IPomodorHandler
{
    event EventHandler<TimeChangedEventArgs>? ElapsedTimeChanged;
    event EventHandler<TimerFinishedEventArgs>? TimerFinished;
    void Start(PomodoroState pomodoroState, double? totalNumberOfSeconds);
    void Stop(double? totalNumberOfSeconds);
    bool IsRunning();
}

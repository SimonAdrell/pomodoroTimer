using CoBySi.Pomodoro.Timer;

namespace CoBySi.Pomodoro;

public interface IPomodorHandler
{
    event EventHandler<TimeChangedEventArgs>? ElapsedTimeChanged;
    void Start();
    void Pause();
    void Resume();
    void Stop();

}

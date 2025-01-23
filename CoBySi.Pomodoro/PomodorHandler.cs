using CoBySi.Pomodoro.Timer;
using Serilog;

namespace CoBySi.Pomodoro;

public class PomodorHandler : IPomodorHandler
{
    public event EventHandler<TimeChangedEventArgs>? ElapsedTimeChanged;
    public event EventHandler<TimerFinishedEventArgs>? TimerFinished;
    private double? _totalNumberOfSeconds { get; set; }
    private PomodoroState _currentState;
    private TimeProvider _timeProvider;
    private int _secondsElapsed;
    private ITimer? _pomodoroTimer;

    public PomodorHandler(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public void Start(PomodoroState pomodoroState, double? totalNumberOfSeconds)
    {
        _currentState = pomodoroState;
        _pomodoroTimer = _timeProvider.CreateTimer(TimerCallback, pomodoroState, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        _secondsElapsed = 0;
        _totalNumberOfSeconds = totalNumberOfSeconds;
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSecondsLeft = _totalNumberOfSeconds - _secondsElapsed, State = _currentState });
        Log.Information("Started {State}", _currentState);
    }
    private void TimerCallback(object? state)
    {
        Tick();
    }

    private void Tick()
    {
        _secondsElapsed++;
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSecondsLeft = _totalNumberOfSeconds - _secondsElapsed, State = _currentState });
        if (_secondsElapsed == _totalNumberOfSeconds)
        {
            StopTimer();
            TimerFinished?.Invoke(this, new TimerFinishedEventArgs { StateFinished = _currentState });
        }
        Log.Information("Tick");

    }

    private void StopTimer()
    {
        _pomodoroTimer?.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }



    public void Stop(double? totalNumberOfSeconds)
    {
        StopTimer();
        _secondsElapsed = 0;
        _totalNumberOfSeconds = totalNumberOfSeconds;
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSecondsLeft = _totalNumberOfSeconds - _secondsElapsed, State = _currentState });
    }
}

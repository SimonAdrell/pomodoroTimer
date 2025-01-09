using CoBySi.Pomodoro.Timer;
using Serilog;

namespace CoBySi.Pomodoro;

public class PomodorHandler : IPomodorHandler
{
    public event EventHandler<TimeChangedEventArgs>? ElapsedTimeChanged;
    public event EventHandler<TimerFinishedEventArgs>? TimerFinished;
    private double? _totalNumberOfSeconds { get; set; }
    private PomodoroState _currentState;
    private PomodoroSettings _pomodoroSettings;
    private TimeProvider _timeProvider;
    private int _secondsElapsed;
    private ITimer? _pomodoroTimer;

    public PomodorHandler(PomodoroSettings pomodoroSettings, TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _pomodoroSettings = pomodoroSettings;
    }

    public void Start(PomodoroState pomodoroState)
    {
        _currentState = pomodoroState;
        _pomodoroTimer = _timeProvider.CreateTimer(TimerCallback, pomodoroState, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        _secondsElapsed = 0;
        _totalNumberOfSeconds = GetTotalNumberOfSeconds(pomodoroState, _pomodoroSettings);
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

    public static double? GetTotalNumberOfSeconds(PomodoroState pomodoroState, PomodoroSettings pomodoroSettings)
    {
        return pomodoroState switch
        {
            PomodoroState.ShortBreak => TimeSpan.FromMinutes(pomodoroSettings.MinutesPerShortBreak).TotalSeconds,
            PomodoroState.LongBreak => TimeSpan.FromMinutes(pomodoroSettings.MinutesPerLongBreak).TotalSeconds,
            _ => (double?)TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds,
        };
    }

    public void Stop()
    {
        StopTimer();
        _secondsElapsed = 0;
        _totalNumberOfSeconds = GetTotalNumberOfSeconds(_currentState, _pomodoroSettings);
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSecondsLeft = _totalNumberOfSeconds - _secondsElapsed, State = _currentState });
    }
}

using System.Timers;
using CoBySi.Pomodoro.Timer;
using Serilog;

namespace CoBySi.Pomodoro;

public class PomodorHandler : IPomodorHandler
{
    public event EventHandler<TimeChangedEventArgs>? ElapsedTimeChanged;
    public event EventHandler<TimerFinishedEventArgs>? TimerFinished;
    private double? TotalNumberOfSeconds { get; set; }
    private PomodoroState _currentState;
    private PomodoroSettings _pomodoroSettings;
    private System.Timers.Timer _timer;
    private int _secondsElapsed;
    private double? _secondsLeft;

    public PomodorHandler(PomodoroSettings pomodoroSettings)
    {
        _pomodoroSettings = pomodoroSettings;
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += TimerElapsed;
    }

    public void Start(PomodoroState pomodoroState)
    {
        _currentState = pomodoroState;
        _secondsElapsed = 0;
        _timer.Stop();
        _timer.Start();
        Log.Information("Started");
        _secondsLeft = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerPomodoro).TotalSeconds;
        SetTotalNumberOfSeconds();
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSecondsElapsed = TotalNumberOfSeconds, State = _currentState });
    }

    private void SetTotalNumberOfSeconds()
    {
        switch (_currentState)
        {
            case PomodoroState.Pomodoro:
                TotalNumberOfSeconds = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerPomodoro).TotalSeconds;
                break;
            case PomodoroState.ShortBreak:
                TotalNumberOfSeconds = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerShortBreak).TotalSeconds;
                break;
            case PomodoroState.LongBreak:
                TotalNumberOfSeconds = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerLongBreak).TotalSeconds;
                break;
        }
    }

    public void Stop()
    {
        _timer.Stop();
        _secondsElapsed = 0;
        _secondsLeft = 0;
        _timer.Enabled = false;
        SetTotalNumberOfSeconds();
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSecondsElapsed = TotalNumberOfSeconds, State = _currentState });
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _secondsElapsed++;
        _secondsLeft = TotalNumberOfSeconds - _secondsElapsed;
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSecondsElapsed = _secondsLeft, State = _currentState });
        if (_secondsElapsed == TotalNumberOfSeconds)
        {
            _timer.Stop();
            TimerFinished?.Invoke(this, new TimerFinishedEventArgs { StateFinished = _currentState });
        }
    }

    ~PomodorHandler()
    {
        _timer.Elapsed -= TimerElapsed;
    }

}

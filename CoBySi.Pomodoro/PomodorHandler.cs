using System.Timers;
using CoBySi.Pomodoro.Timer;
using Serilog;

namespace CoBySi.Pomodoro;

public class PomodorHandler : IPomodorHandler
{
    public event EventHandler<TimeChangedEventArgs>? ElapsedTimeChanged;
    private PomodoroState _currentState;
    private PomodoroSettings _pomodoroSettings;
    private System.Timers.Timer _timer;
    private int _secondsElapsed;
    private double? _secondsLeft;
    private int _breakCount;

    public PomodorHandler(PomodoroSettings pomodoroSettings)
    {
        _pomodoroSettings = pomodoroSettings;
        _currentState = PomodoroState.Work;
        _timer = new System.Timers.Timer(1000);
        _secondsLeft = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerPomodoro).TotalSeconds;
        _timer.Elapsed += TimerElapsed;
    }

    public void Start()
    {
        _timer.Start();
        Log.Information("Starting timer: {timer}", _pomodoroSettings.MinutesPerPomodoro);
        _currentState = PomodoroState.Work;
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSeconds = _secondsLeft, State = _currentState });
    }

    public void Pause()
    {
        Log.Information("Pomodoro Paused");
        _timer.Stop();
        _currentState = PomodoroState.Paused;
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSeconds = _secondsLeft, State = _currentState });
    }

    public void Resume()
    {
        Log.Information("Pomodoro Work");
        _timer.Start();
        _currentState = PomodoroState.Work;
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSeconds = _secondsLeft, State = _currentState });
    }

    public void Stop()
    {
        Log.Information("Pomodoro stopped");
        _timer.Stop();
        _secondsElapsed = 0;
        _secondsLeft = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerPomodoro).TotalSeconds;
        _currentState = PomodoroState.NotStarted;
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSeconds = _secondsLeft, State = _currentState });
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Log.Information("Elapsed time: {workCound} {currentState}", _secondsElapsed, _currentState);
        _secondsLeft = null;
        switch (_currentState)
        {
            case PomodoroState.Work:
                _secondsElapsed++;
                _secondsLeft = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerPomodoro).TotalSeconds - _secondsElapsed;
                if (_secondsElapsed == TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerPomodoro).TotalSeconds)
                {
                    _secondsElapsed = 0;
                    if (_breakCount == _pomodoroSettings.PomodorosBeforeLongBreak)
                    {
                        _currentState = PomodoroState.LongBreak;
                        _breakCount = 0;
                    }
                    else
                    {
                        _currentState = PomodoroState.ShortBreak;
                        _breakCount++;
                    }
                }
                break;
            case PomodoroState.ShortBreak:
                _secondsLeft = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerShortBreak).TotalSeconds - _secondsElapsed;
                if (_secondsElapsed == TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerShortBreak).TotalSeconds)
                {
                    _secondsElapsed = 0;
                    _currentState = PomodoroState.Work;
                }
                break;
            case PomodoroState.LongBreak:
                _secondsLeft = TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerLongBreak).TotalSeconds - _secondsElapsed;
                if (_secondsElapsed == TimeSpan.FromMinutes(_pomodoroSettings.MinutesPerPomodoro).TotalSeconds)
                {
                    _currentState = 0;
                    _currentState = PomodoroState.Work;
                }
                break;
        }
        ElapsedTimeChanged?.Invoke(this, new TimeChangedEventArgs { NumberOfSeconds = _secondsLeft, State = _currentState });

    }

    ~PomodorHandler()
    {
        _timer.Elapsed -= TimerElapsed;
    }

}

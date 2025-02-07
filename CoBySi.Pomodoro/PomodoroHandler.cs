using CoBySi.Pomodoro.Models;
using CoBySi.Pomodoro.Timer;
using Serilog;

namespace CoBySi.Pomodoro;

public class PomodorHandler : IPomodorHandler
{
    public event EventHandler<TimerChangedEventArgs>? TimerChanged;
    private double? _totalNumberOfSeconds { get; set; }
    private TimeProvider _timeProvider;
    private int _secondsElapsed;
    private ITimer? _pomodoroTimer;
    private PomodoroItem? _currentItem;
    private PomodoroSettings? _pomodoroSettings;

    public PomodorHandler(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public bool IsRunning()
    {
        return _pomodoroTimer != null;
    }

    public void StartNext(PomodoroSettings? pomodoroSettings)
    {
        if (IsRunning())
            StopTimer();

        _pomodoroSettings = pomodoroSettings;
        _currentItem ??= new PomodoroItem
        {
            Status = PomodoroStatus.Pomodoro,
            NumberOfPomodoros = 0,
            TotalNumberOfSeconds = GetTotalNumberOfSeconds(PomodoroStatus.Pomodoro, pomodoroSettings),
            Id = Guid.NewGuid()
        };
        _pomodoroTimer = _timeProvider.CreateTimer(TimerCallback, _currentItem, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        _secondsElapsed = 0;
        _totalNumberOfSeconds = _currentItem.TotalNumberOfSeconds;
        Log.Information("Started {id} {State}", _currentItem.Id, _currentItem.Status);
    }
    private void TimerCallback(object? state)
    {
        if (state is PomodoroItem pomodoroItem)
            Tick(pomodoroItem);
    }

    private void Tick(PomodoroItem pomodoroItem)
    {
        if (_secondsElapsed == _totalNumberOfSeconds)
        {
            StopTimer();
            pomodoroItem = TimerFinished(pomodoroItem);

            TimerChanged?.Invoke(this,
               new TimerChangedEventArgs { NumberOfSecondsLeft = pomodoroItem.TotalNumberOfSeconds, Item = pomodoroItem, EventType = TimerEventType.Finished });
            _secondsElapsed++;
            return;
        }
        else
        {
            TimerChanged?.Invoke(this,
               new TimerChangedEventArgs { NumberOfSecondsLeft = _totalNumberOfSeconds - _secondsElapsed, Item = pomodoroItem, EventType = TimerEventType.Tick });
            _secondsElapsed++;
        }
        Log.Information("Tick {id} Seconds elapseds {_secondsElapsed}", pomodoroItem.Id, _secondsElapsed);
    }

    private PomodoroItem TimerFinished(PomodoroItem pomodoroItem)
    {
        if (pomodoroItem.Status == PomodoroStatus.Pomodoro)
        {
            if (pomodoroItem.NumberOfPomodoros >= _pomodoroSettings?.PomodorosBeforeLongBreak)
            {
                pomodoroItem.Status = PomodoroStatus.LongBreak;
                pomodoroItem.NumberOfPomodoros = 0;
                return pomodoroItem;    
            }

            pomodoroItem.Status = PomodoroStatus.ShortBreak;
            pomodoroItem.NumberOfPomodoros++;
        }
        else
        {
            pomodoroItem.Status = PomodoroStatus.Pomodoro;
        }
        pomodoroItem.TotalNumberOfSeconds = GetTotalNumberOfSeconds(pomodoroItem.Status, _pomodoroSettings);
        return pomodoroItem;
    }
    private void StopTimer()
    {
        _pomodoroTimer?.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        _pomodoroTimer?.Dispose();
        _pomodoroTimer = null;
    }

    public void Stop()
    {
        StopTimer();
        _secondsElapsed = 0;
    }


    public static double GetTotalNumberOfSeconds(PomodoroStatus pomodoroState, PomodoroSettings pomodoroSettings)
    {
        return pomodoroState switch
        {
            PomodoroStatus.ShortBreak => TimeSpan.FromMinutes(pomodoroSettings.MinutesPerShortBreak).TotalSeconds,
            PomodoroStatus.LongBreak => TimeSpan.FromMinutes(pomodoroSettings.MinutesPerLongBreak).TotalSeconds,
            _ => (double)TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds,
        };
    }
}

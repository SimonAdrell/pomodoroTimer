using CoBySi.Pomodoro.Timer;
using Microsoft.Extensions.Time.Testing;

namespace CoBySi.Pomodoro.Tests;

public class PomodorHandlerTest
{
    [Fact]
    public void Start_WhenCalled_ShouldRaiseElapsedTimeChangedEvent()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var pomodoroSettings = new PomodoroSettings
        {
            MinutesPerPomodoro = 1
        };
        var pomodoroHandler = new PomodorHandler(timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        var raised = false;

        double? totalNumberOfSecondsLeft = 0;
        pomodoroHandler.ElapsedTimeChanged += (sender, args) =>
        {
            raised = true;
            totalNumberOfSecondsLeft = args.NumberOfSecondsLeft;
        };

        // Act
        pomodoroHandler.Start(pomodoroState, TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds);

        // Assert
        Assert.True(raised);
        Assert.Equal(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds, totalNumberOfSecondsLeft);
    }

    [Fact]
    public void Start_WhenFinished_ShouleRaiseTimerFinnishedEvent()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var pomodoroSettings = new PomodoroSettings
        {
            MinutesPerPomodoro = 25
        };
        var pomodoroHandler = new PomodorHandler(timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        var raised = false;
        pomodoroHandler.TimerFinished += (sender, args) => raised = true;

        // Act
        pomodoroHandler.Start(pomodoroState, TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds);
        timeProvider.Advance(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro));

        // Assert
        Assert.True(raised);
    }

    [Fact]
    public void Start_WhenFinished_ShouldRaiseElapsedTimeChangedEvent()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var pomodoroSettings = new PomodoroSettings
        {
            MinutesPerPomodoro = 25
        };
        var pomodoroHandler = new PomodorHandler(timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        var raised = false;
        int ticks = 0;
        pomodoroHandler.TimerFinished += (sender, args) => raised = true;
        pomodoroHandler.ElapsedTimeChanged += (sender, args) => ticks++;

        // Act
        pomodoroHandler.Start(pomodoroState, TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds);
        timeProvider.Advance(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro));

        // Assert
        Assert.True(raised);
        Assert.Equal(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds + 2, ticks);

    }

    [Fact]
    public void Stop_StartedTimer_StopsTicking()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var pomodoroSettings = new PomodoroSettings
        {
            MinutesPerPomodoro = 25
        };
        var pomodoroHandler = new PomodorHandler(timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        int ticks = 0;
        pomodoroHandler.ElapsedTimeChanged += (sender, args) => ticks++;

        // Act
        pomodoroHandler.Start(pomodoroState, TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds);
        timeProvider.Advance(TimeSpan.FromMinutes(1));

        pomodoroHandler.Stop(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds);

        timeProvider.Advance(TimeSpan.FromMinutes(3));


        // Assert
        Assert.InRange(ticks, 0, TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds - 10);
    }



}


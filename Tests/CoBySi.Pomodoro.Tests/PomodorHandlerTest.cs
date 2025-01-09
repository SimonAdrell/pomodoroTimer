using System.Linq.Expressions;
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
        var pomodoroHandler = new PomodorHandler(pomodoroSettings, timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        var raised = false;
        double? totalNumberOfSecondsLeft = 0;
        pomodoroHandler.ElapsedTimeChanged += (sender, args) =>
        {
            raised = true;
            totalNumberOfSecondsLeft = args.NumberOfSecondsLeft;
        };

        // Act
        pomodoroHandler.Start(pomodoroState);

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
        var pomodoroHandler = new PomodorHandler(pomodoroSettings, timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        var raised = false;
        pomodoroHandler.TimerFinished += (sender, args) => raised = true;

        // Act
        pomodoroHandler.Start(pomodoroState);
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
        var pomodoroHandler = new PomodorHandler(pomodoroSettings, timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        var raised = false;
        int ticks = 0;
        pomodoroHandler.TimerFinished += (sender, args) => raised = true;
        pomodoroHandler.ElapsedTimeChanged += (sender, args) => ticks++;

        // Act
        pomodoroHandler.Start(pomodoroState);
        timeProvider.Advance(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro));

        // Assert
        Assert.True(raised);
        Assert.Equal(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds + 2, ticks);

    }
    [Theory]
    [InlineData(PomodoroState.Pomodoro, 1500)]
    [InlineData(PomodoroState.ShortBreak, 300)]
    [InlineData(PomodoroState.LongBreak, 900)]
    public void GetTotalNumberOfSeconds_ShouldReturnCorrectValue(PomodoroState state, double expectedSeconds)
    {
        // Arrange
        var pomodoroSettings = new PomodoroSettings
        {
            MinutesPerPomodoro = 25,
            MinutesPerShortBreak = 5,
            MinutesPerLongBreak = 15
        };

        // Act
        var result = PomodorHandler.GetTotalNumberOfSeconds(state, pomodoroSettings);

        // Assert
        Assert.Equal(expectedSeconds, result);
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
        var pomodoroHandler = new PomodorHandler(pomodoroSettings, timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        int ticks = 0;
        pomodoroHandler.ElapsedTimeChanged += (sender, args) => ticks++;

        // Act
        pomodoroHandler.Start(pomodoroState);
        timeProvider.Advance(TimeSpan.FromMinutes(1));

        pomodoroHandler.Stop();

        timeProvider.Advance(TimeSpan.FromMinutes(3));


        // Assert
        Assert.InRange(ticks, 0, TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds - 10);
    }



}


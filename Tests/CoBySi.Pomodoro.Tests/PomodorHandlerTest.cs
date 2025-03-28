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
        var raised = false;

        double? totalNumberOfSecondsLeft = 0;
        pomodoroHandler.TimerChangedAsync += async (sender, args) =>
        {
            raised = true;
            totalNumberOfSecondsLeft = args.NumberOfSecondsLeft;
            await Task.CompletedTask;
        };

        // Act  
        pomodoroHandler.StartNext(pomodoroSettings);

        // Assert
        Assert.True(raised);
        Assert.Equal(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds, totalNumberOfSecondsLeft);
    }

    [Fact]
    public void Start_WhenFinished_ShouldRaiseTimerFinishedEvent()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var pomodoroSettings = new PomodoroSettings
        {
            MinutesPerPomodoro = 25
        };
        var pomodoroHandler = new PomodorHandler(timeProvider);
        var raised = false;
        pomodoroHandler.TimerChangedAsync += async (sender, args) =>
        {
            raised = args.EventType.Equals(Models.TimerEventType.Finished);
            await Task.CompletedTask;
        };

        // Act
        pomodoroHandler.StartNext(pomodoroSettings);
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
        var raised = false;
        int ticks = 0;
        pomodoroHandler.TimerChangedAsync += async (sender, args) =>
        {
            raised = args.EventType.Equals(Models.TimerEventType.Finished);
            await Task.CompletedTask;
        };
        pomodoroHandler.TimerChangedAsync += async (sender, args) =>
        {
            ticks++;
            await Task.CompletedTask;
        };

        // Act
        pomodoroHandler.StartNext(pomodoroSettings);
        timeProvider.Advance(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro));

        // Assert
        Assert.True(raised);
        Assert.Equal(TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds + 1, ticks);

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
        int ticks = 0;
        pomodoroHandler.TimerChangedAsync += async (sender, args) => { ticks++; await Task.CompletedTask; };



        // Act
        pomodoroHandler.StartNext(pomodoroSettings);
        timeProvider.Advance(TimeSpan.FromMinutes(1));

        pomodoroHandler.Stop();

        timeProvider.Advance(TimeSpan.FromMinutes(3));


        // Assert

        Assert.InRange(ticks, 0, TimeSpan.FromMinutes(pomodoroSettings.MinutesPerPomodoro).TotalSeconds - 10);
    }

}
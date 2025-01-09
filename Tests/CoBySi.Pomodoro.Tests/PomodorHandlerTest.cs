using System;
using System.Runtime.CompilerServices;
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
        var pomodoroSettings = new PomodoroSettings();
        var pomodoroHandler = new PomodorHandler(pomodoroSettings, timeProvider);
        var pomodoroState = PomodoroState.Pomodoro;
        var raised = false;
        pomodoroHandler.ElapsedTimeChanged += (sender, args) => raised = true;

        // Act
        pomodoroHandler.Start(pomodoroState);

        // Assert
        Assert.True(raised);
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

}

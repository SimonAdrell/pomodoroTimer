using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Timer;
using CoBySi.Pomodoro.Web.Components;
using Xunit;

namespace CoBySi.PomodorTimer.Web.Tests.Components.Pages;

public class PomodoroTests
{
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
        var result = PomodoroComponent.GetTotalNumberOfSeconds(state, pomodoroSettings);

        // Assert
        Xunit.Assert.Equal(expectedSeconds, result);
    }

}

using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Web.PomodoroProperties;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using Assert = Xunit.Assert;

namespace CoBySi.PomodorTimer.Web.Tests.PomodoroProperties;

public class PomdoroPropertiesHandlerTest
{
    [Fact]
    public async Task GetPomodoroPropertiesAsync_EmptyGuid_ReturnsDefault()
    {
        // Arrange
        var defaultPomodoroSettings = new PomodoroSettings()
        {
            MinutesPerLongBreak = 15,
            MinutesPerShortBreak = 5,
            MinutesPerPomodoro = 25,
            PomodorosBeforeLongBreak = 4
        };

        var pomodoroSettings = Substitute.For<IOptions<PomodoroSettings>>();
        pomodoroSettings.Value.Returns(defaultPomodoroSettings);

        var stubUserSettingsRepository = Substitute.For<IUserSettingsRepository>();

        var sut = new PomdoroPropertiesHandler(pomodoroSettings, stubUserSettingsRepository);

        // Act
        var response = await sut.GetPomodoroPropertiesAsync(Guid.Empty);

        // Assert
        Assert.Equal(defaultPomodoroSettings, response);
        await stubUserSettingsRepository.DidNotReceiveWithAnyArgs().GetUserPomodoroSettingsAsync(default);
    }

    [Fact]
    public async Task GetPomodoroPropertiesAsync_ValidUserGuid_ReturnsUsersSettinngs()
    {
        // Arrange
        var userGuid = Guid.NewGuid();

        var userSettings = new UserPomodoroSettingsEntity()
        {
            MinutesPerLongBreak = 15,
            MinutesPerPomodoro = 25,
            MinutesPerShortBreak = 5,
            PomodorosBeforeLongBreak = 4
        };

        var defaultPomodoroSettings = new PomodoroSettings()
        {
            MinutesPerLongBreak = 0,
            MinutesPerShortBreak = 0,
            MinutesPerPomodoro = 0,
            PomodorosBeforeLongBreak = 0
        };
        var pomodoroSettings = Substitute.For<IOptions<PomodoroSettings>>();
        pomodoroSettings.Value.Returns(defaultPomodoroSettings);

        var stubUserSettingsRepository = Substitute.For<IUserSettingsRepository>();
        stubUserSettingsRepository.GetUserPomodoroSettingsAsync(userGuid).Returns(userSettings);
        var sut = new PomdoroPropertiesHandler(pomodoroSettings, stubUserSettingsRepository);

        // Act
        var response = await sut.GetPomodoroPropertiesAsync(userGuid);

        // Asssert
        Assert.NotEqual(defaultPomodoroSettings, response);
        await stubUserSettingsRepository.Received().GetUserPomodoroSettingsAsync(userGuid);
    }
}

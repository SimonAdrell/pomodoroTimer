using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Web.Cache;
using CoBySi.Pomodoro.Web.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace CoBySi.PomodorTimer.Web.Tests.Services;

public class PomodoroSettingsServiceTest
{

    [Fact]
    public async Task GetUserPomodoroSettingsAsync_ReturnsDefaultSettings_WhenUserIdIsNullOrEmpty()
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

        var userSettingsRepository = Substitute.For<IUserSettingsRepository>();

        var settingsCache = Substitute.For<ISettingsCache>();

        var sut = new PomodoroSettingsService(pomodoroSettings, userSettingsRepository, settingsCache);
        string userId = string.Empty;
        string sessionId = string.Empty;
        // Act

        var result = await sut.GetUserPomodoroSettingsAsync(userId, sessionId, CancellationToken.None);

        // Assert
        Assert.Equal(defaultPomodoroSettings, result);
    }

    [Fact]
    public async Task GetUserPomodoroSettingsAsync_ReturnsDefaultSettings_WhenUserSettingsNotFound()
    {
        // Arrange
        string userId = "testUser";
        var defaultPomodoroSettings = new PomodoroSettings()
        {
            MinutesPerLongBreak = 15,
            MinutesPerShortBreak = 5,
            MinutesPerPomodoro = 25,
            PomodorosBeforeLongBreak = 4
        };

        var pomodoroSettings = Substitute.For<IOptions<PomodoroSettings>>();
        pomodoroSettings.Value.Returns(defaultPomodoroSettings);

        var userSettingsRepository = Substitute.For<IUserSettingsRepository>();
        userSettingsRepository.GetUserPomodoroSettingsAsync(userId, Arg.Any<CancellationToken>()).Returns((UserPomodoroSettingsEntity?)null);

        var settingsCache = Substitute.For<ISettingsCache>();
        var sut = new PomodoroSettingsService(pomodoroSettings, userSettingsRepository, settingsCache);

        var sessionId = string.Empty;
        // Act
        var result = await sut.GetUserPomodoroSettingsAsync(userId, sessionId, CancellationToken.None);

        // Assert
        Assert.Equal(defaultPomodoroSettings, result);
    }

    [Fact]
    public async Task GetUserPomodoroSettingsAsync_ReturnsUserSettings_WhenUserSettingsFound()
    {
        // Arrange
        string userId = "testUser";
        var defaultPomodoroSettings = new PomodoroSettings()
        {
            MinutesPerLongBreak = 15,
            MinutesPerShortBreak = 5,
            MinutesPerPomodoro = 25,
            PomodorosBeforeLongBreak = 4
        };

        var userPomodoroSettingsEntity = new UserPomodoroSettingsEntity()
        {
            UserID = userId,
            MinutesPerLongBreak = 20,
            MinutesPerShortBreak = 10,
            MinutesPerPomodoro = 30,
            PomodorosBeforeLongBreak = 5
        };

        var pomodoroSettings = Substitute.For<IOptions<PomodoroSettings>>();
        pomodoroSettings.Value.Returns(defaultPomodoroSettings);

        var userSettingsRepository = Substitute.For<IUserSettingsRepository>();
        userSettingsRepository.GetUserPomodoroSettingsAsync(userId, Arg.Any<CancellationToken>()).Returns(userPomodoroSettingsEntity);

        var settingsCache = Substitute.For<ISettingsCache>();
        var sut = new PomodoroSettingsService(pomodoroSettings, userSettingsRepository, settingsCache);

        var sessionId = string.Empty;
        // Act
        var result = await sut.GetUserPomodoroSettingsAsync(userId, sessionId, CancellationToken.None);

        // Assert
        Assert.Equal(userPomodoroSettingsEntity.MinutesPerLongBreak, result.MinutesPerLongBreak);
        Assert.Equal(userPomodoroSettingsEntity.MinutesPerShortBreak, result.MinutesPerShortBreak);
        Assert.Equal(userPomodoroSettingsEntity.MinutesPerPomodoro, result.MinutesPerPomodoro);
        Assert.Equal(userPomodoroSettingsEntity.PomodorosBeforeLongBreak, result.PomodorosBeforeLongBreak);

    }
}


using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.Repositories;
using CoBySi.Pomodoro.Repository.Repositories.Cache;
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

        var userSettingsRepository = Substitute.For<ISettingsRepository>();

        var settingsCache = Substitute.For<ISettingsCache>();

        var sut = new SettingsService(pomodoroSettings, userSettingsRepository, settingsCache);
        string userId = string.Empty;

        // Act
        var result = await sut.GetUserPomodoroSettingsAsync(userId, CancellationToken.None);

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

        var userSettingsRepository = Substitute.For<ISettingsRepository>();
        userSettingsRepository.GetUserSettingsByUserId(userId, Arg.Any<CancellationToken>()).Returns((UserSettings?)null);

        var settingsCache = Substitute.For<ISettingsCache>();
        var sut = new SettingsService(pomodoroSettings, userSettingsRepository, settingsCache);

        // Act
        var result = await sut.GetUserPomodoroSettingsAsync(userId, CancellationToken.None);

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

        var userPomodoroSettingsEntity = new PomodoroSettingsEntity()
        {
            MinutesPerLongBreak = 20,
            MinutesPerShortBreak = 10,
            MinutesPerPomodoro = 30,
            PomodorosBeforeLongBreak = 5
        };

        var userSettings = new UserSettings(userId)
        {
            NotificationEntity = null,
            PomodoroSettings = userPomodoroSettingsEntity
        };

        var pomodoroSettings = Substitute.For<IOptions<PomodoroSettings>>();
        pomodoroSettings.Value.Returns(defaultPomodoroSettings);

        var userSettingsRepository = Substitute.For<ISettingsRepository>();
        userSettingsRepository.GetUserSettingsByUserId(userId, Arg.Any<CancellationToken>()).Returns(userSettings);

        var settingsCache = Substitute.For<ISettingsCache>();
        var sut = new SettingsService(pomodoroSettings, userSettingsRepository, settingsCache);

        // Act
        var result = await sut.GetUserPomodoroSettingsAsync(userId, CancellationToken.None);

        // Assert
        Assert.Equal(userPomodoroSettingsEntity.MinutesPerLongBreak, result.MinutesPerLongBreak);
        Assert.Equal(userPomodoroSettingsEntity.MinutesPerShortBreak, result.MinutesPerShortBreak);
        Assert.Equal(userPomodoroSettingsEntity.MinutesPerPomodoro, result.MinutesPerPomodoro);
        Assert.Equal(userPomodoroSettingsEntity.PomodorosBeforeLongBreak, result.PomodorosBeforeLongBreak);

    }
}


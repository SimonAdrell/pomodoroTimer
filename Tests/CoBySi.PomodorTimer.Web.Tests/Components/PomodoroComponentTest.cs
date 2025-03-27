using Bunit;
using Bunit.TestDoubles;
using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Models;
using CoBySi.Pomodoro.Repository.Identity.Data;
using CoBySi.Pomodoro.Repository.Repositories;
using CoBySi.Pomodoro.Timer;
using CoBySi.Pomodoro.Web.Components;
using CoBySi.Pomodoro.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CoBySi.PomodorTimer.Web.Tests.Components;

public class PomodoroComponentTest
{
    [Fact]
    public async Task FinishedTimer_SendsNotifcations()
    {
        // Arrange
        var pomodoroSetingsService = Substitute.For<ISettingsService>();
        var userSettingsRepository = Substitute.For<ISettingsRepository>();
        var notificationService = Substitute.For<INotificationService>();

        var pomodorHandler = Substitute.For<IPomodorHandler>();
        var localStorageService = Substitute.For<ILocalStorageService>();

        var userStore = Substitute.For<IUserStore<PomodoroUser>>();
        userStore.GetUserIdAsync(Arg.Any<PomodoroUser>(), Arg.Any<CancellationToken>()).Returns("1");

        var UserManager = Substitute.For<UserManager<PomodoroUser>>(userStore, null, null, null, null, null, null, null, null);

        using var ctx = new TestContext();
        ctx.AddTestAuthorization();

        pomodoroSetingsService
            .GetUserPomodoroSettingsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new PomodoroSettings() { MinutesPerPomodoro = 25, MinutesPerShortBreak = 5, MinutesPerLongBreak = 15, PomodorosBeforeLongBreak = 4 });

        ctx.Services.AddSingleton(pomodoroSetingsService);
        ctx.Services.AddSingleton(userSettingsRepository);
        ctx.Services.AddSingleton(pomodorHandler);
        ctx.Services.AddSingleton(notificationService);
        ctx.Services.AddSingleton(localStorageService);
        ctx.Services.AddSingleton(UserManager);

        var stateProvider = ctx.Services.GetService<AuthenticationStateProvider>();

        // Act

        var cut = ctx.RenderComponent<PomodoroComponent>();

        var buttonElement = cut.Find("#startBtn");
        buttonElement.Click();


        pomodorHandler.TimerChangedAsync += Raise.Event<AsyncEventHandler<TimerChangedEventArgs>>(new TimerChangedEventArgs() { NumberOfSecondsLeft = 0, EventType = TimerEventType.Finished, Item = new PomodoroItem() { Status = PomodoroStatus.Pomodoro } });


        // Assert
        await notificationService.Received(1)
            .InvokeNotificationShow(Arg.Is<string>(s => s.Equals("Pomodoro Complete!")), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());

        await notificationService.Received(1)
            .InvokeNotificationPermissionAsync();

        await notificationService.Received(1)
            .PlayCompletionSoundAsync();

    }

    [Theory]
    [InlineData(PomodoroStatus.Pomodoro, "Pomodoro Complete!")]
    [InlineData(PomodoroStatus.LongBreak, "Break Complete!")]
    [InlineData(PomodoroStatus.ShortBreak, "Break Complete!")]
    public async Task NotifyTimerCompletion_PomodoroStatus_CorrectTitle(PomodoroStatus pomodoroStatus, string expectedTitle)
    {
        // Arrange
        var pomodoroSettingsService = Substitute.For<ISettingsService>();
        var userSettingsRepository = Substitute.For<ISettingsRepository>();
        var notificationService = Substitute.For<INotificationService>();

        var pomodoroComponent = new PomodoroComponent(pomodoroSettingsService, userSettingsRepository, notificationService);
        var pomodoroItem = new PomodoroItem() { Status = pomodoroStatus };

        // Act 
        await pomodoroComponent.NotifyTimerCompletion(pomodoroItem);

        // Assert
        await notificationService.Received(1)
            .InvokeNotificationShow(Arg.Is<string>(s => s.Equals(expectedTitle)), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(PomodoroStatus.Pomodoro, "Time to take a break!")]
    [InlineData(PomodoroStatus.LongBreak, "Time to work!")]
    [InlineData(PomodoroStatus.ShortBreak, "Time to work!")]
    public async Task NotifyTimerCompletion_PomodoroStatus_CorrectBody(PomodoroStatus pomodoroStatus, string expectedTitle)
    {
        // Arrange
        var pomodoroSettingsService = Substitute.For<ISettingsService>();
        var userSettingsRepository = Substitute.For<ISettingsRepository>();
        var notificationService = Substitute.For<INotificationService>();

        var pomodoroComponent = new PomodoroComponent(pomodoroSettingsService, userSettingsRepository, notificationService);
        var pomodoroItem = new PomodoroItem() { Status = pomodoroStatus };

        // Act 
        await pomodoroComponent.NotifyTimerCompletion(pomodoroItem);

        // Assert
        await notificationService.Received(1)
            .InvokeNotificationShow(Arg.Any<string>(), Arg.Is<string>(s => s.Equals(expectedTitle)), Arg.Any<string>(), Arg.Any<string>());
    }
}

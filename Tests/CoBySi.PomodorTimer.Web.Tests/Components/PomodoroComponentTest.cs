using System;
using System.Threading.Tasks;
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
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using NSubstitute;

namespace CoBySi.PomodorTimer.Web.Tests.Components;

public class PomodoroComponentTest
{
    [Fact]
    public async Task FinishedTimer_SendsNotifcations()
    {
        // Arrange
        var pomodoroSetingsService = Substitute.For<IPomodoroSettingsService>();
        var userSettingsRepository = Substitute.For<IUserSettingsRepository>();
        var jsRuntime = Substitute.For<IJSRuntime>();
        var pomodorHandler = Substitute.For<IPomodorHandler>();
        var localStorageService = Substitute.For<ILocalStorageService>();

        var userStore = Substitute.For<IUserStore<PomodoroUser>>();
        userStore.GetUserIdAsync(Arg.Any<PomodoroUser>(), Arg.Any<System.Threading.CancellationToken>()).Returns("1");

        var UserManager = Substitute.For<UserManager<PomodoroUser>>(userStore, null, null, null, null, null, null, null, null);

        using var ctx = new TestContext();
        ctx.AddTestAuthorization();

        pomodoroSetingsService
            .GetUserPomodoroSettingsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new PomodoroSettings() { MinutesPerPomodoro = 25, MinutesPerShortBreak = 5, MinutesPerLongBreak = 15, PomodorosBeforeLongBreak = 4 });

        ctx.Services.AddSingleton(pomodoroSetingsService);
        ctx.Services.AddSingleton(userSettingsRepository);
        ctx.Services.AddSingleton(pomodorHandler);
        ctx.Services.AddSingleton(jsRuntime);
        ctx.Services.AddSingleton(localStorageService);
        ctx.Services.AddSingleton(UserManager);

        var stateProvider = ctx.Services.GetService<AuthenticationStateProvider>();

        // Act

        var cut = ctx.RenderComponent<PomodoroComponent>();

        var buttonElement = cut.Find("#startBtn");
        buttonElement.Click();

        pomodorHandler.TimerChanged += Raise.EventWith(new TimerChangedEventArgs() { NumberOfSecondsLeft = 0, EventType = TimerEventType.Finished, Item = new PomodoroItem() { Status = PomodoroStatus.Pomodoro } });


        // Assert
        await jsRuntime.Received(1).InvokeVoidAsync(Arg.Is<string>(s => s.Equals("requestNotificationPermission")));
        await jsRuntime.Received(1).InvokeVoidAsync(Arg.Is<string>(s => s.Equals("showNotification")), Arg.Any<object[]>());
        await jsRuntime.Received(1).InvokeVoidAsync("playSound", Arg.Any<object[]>());
    }
}

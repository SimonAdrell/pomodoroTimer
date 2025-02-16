using System;
using Microsoft.JSInterop;
using Serilog;

namespace CoBySi.Pomodoro.Web.Services;

public class NotificaionService : INotificationService
{
    private readonly IServiceProvider _serviceProvider;

    public NotificaionService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    private IJSRuntime JSRuntime => (IJSRuntime?)_serviceProvider.GetService(typeof(IJSRuntime))
        ?? throw new InvalidOperationException("IJSRuntime is not registered.");

    public async Task InvokeNotificaionShow(string title, string body, string iconUrl, string? tag = null)
    {
        Log.Information("InvokeNotificaionShow {title} {body} {iconUrl} {tag}", title, body, iconUrl, tag);
        await JSRuntime.InvokeVoidAsync(
        "showNotification",
        title,
        new { body, icon = iconUrl, tag }
        );

    }

    public async Task InvokeNotificationPermissionAsync()
    {
        await JSRuntime.InvokeVoidAsync("requestNotificationPermission");
    }

    public async Task PlayCompletionSoundAsync()
    {
        await JSRuntime.InvokeVoidAsync("playSound", "timerSound");
    }
}

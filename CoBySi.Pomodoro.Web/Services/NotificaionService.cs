using Microsoft.JSInterop;

namespace CoBySi.Pomodoro.Web.Services;

public class NotificationService : INotificationService
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    private IJSRuntime JSRuntime => (IJSRuntime?)_serviceProvider.GetService(typeof(IJSRuntime))
        ?? throw new InvalidOperationException("IJSRuntime is not registered.");

    public async Task InvokeNotificationShow(string title, string body, string iconUrl, string? tag = null)
    {
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

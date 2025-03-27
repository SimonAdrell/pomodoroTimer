namespace CoBySi.Pomodoro.Web.Services;

public interface INotificationService
{
    Task InvokeNotificationPermissionAsync();
    Task InvokeNotificationShow(string title, string body, string iconUrl, string? tag = null);
    Task PlayCompletionSoundAsync();
}

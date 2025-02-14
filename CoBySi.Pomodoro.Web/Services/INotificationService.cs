namespace CoBySi.Pomodoro.Web.Services;

public interface INotificationService
{
    Task InvokeNotificationPermissionAsync();
    Task InvokeNotificaionShow(string title, string body, string iconUrl, string? tag = null);
    Task PlayCompletionSoundAsync();
}

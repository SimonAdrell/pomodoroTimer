using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CoBySi.Pomodoro.Web.Services;

public class LocalStorageService : ILocalStorageService
{
    private const string SessionCookieName = "PomodoroSessionId";
    private readonly IServiceProvider _serviceProvider;

    public LocalStorageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> GetOrCreateSessionIdAsync()
    {
        var localStorage = (ProtectedLocalStorage?)_serviceProvider.GetService(typeof(ProtectedLocalStorage))
                ?? throw new InvalidOperationException("ProtectedLocalStorage is not registered.");

        var session = await localStorage.GetAsync<Guid>(SessionCookieName);
        if (session.Success && session.Value != Guid.Empty)
            return session.Value.ToString();

        var newSessionId = Guid.NewGuid().ToString();
        await localStorage.SetAsync(SessionCookieName, newSessionId);
        return newSessionId;
    }
}

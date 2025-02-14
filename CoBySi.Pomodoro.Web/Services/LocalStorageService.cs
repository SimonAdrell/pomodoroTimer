using System;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CoBySi.Pomodoro.Web.Services;

public class LocalStorageService : ILocalStorageService
{
    private const string SessionCookieName = "PomodoroSessionId";
    private readonly ProtectedLocalStorage _localStorage;

    public LocalStorageService(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string> GetOrCreateSessionIdAsync()
    {
        var session = await _localStorage.GetAsync<Guid>(SessionCookieName);
        if (session.Success && session.Value != Guid.Empty)
            return session.Value.ToString();

        var newSessionId = Guid.NewGuid().ToString();
        await _localStorage.SetAsync(SessionCookieName, newSessionId);
        return newSessionId;
    }
}

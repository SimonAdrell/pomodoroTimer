using System;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CoBySi.Pomodoro.Web.Handler;

public static class SessionHandler
{
    private const string SessionCookieName = "PomodoroSessionId";
    public static async Task<string> GetOrCreateSessionIdAsync(this ProtectedLocalStorage protectedLocalStorage)
    {
        var session = await protectedLocalStorage.GetAsync<Guid>(SessionCookieName);
        if (session.Success && session.Value != Guid.Empty)
            return session.Value.ToString();

        var newSessionId = Guid.NewGuid().ToString();
        await protectedLocalStorage.SetAsync(SessionCookieName, newSessionId);
        return newSessionId;
    }
}

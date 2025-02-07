using System;

namespace CoBySi.Pomodoro.Web.Handler;

public class SessionHandler
{
    private const string SessionCookieName = "PomodoroSessionId";

    public static string GetOrCreateSessionId(HttpContext? context)
    {
        if (context == null)
            throw new InvalidOperationException("HttpContext is not available.");

        if (context.Request.Cookies.TryGetValue(SessionCookieName, out var existingSessionId))
            return existingSessionId;

        var newSessionId = Guid.NewGuid().ToString();
        context.Response.Cookies.Append(SessionCookieName, newSessionId, new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            Expires = DateTimeOffset.UtcNow.AddMonths(1)
        });

        return newSessionId;
    }
}

using System;

namespace CoBySi.Pomodoro.Web.Services;

public interface ILocalStorageService
{
    Task<string> GetOrCreateSessionIdAsync();
}

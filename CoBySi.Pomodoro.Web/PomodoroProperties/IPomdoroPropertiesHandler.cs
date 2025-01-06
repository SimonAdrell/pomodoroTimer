using System;

namespace CoBySi.Pomodoro.Web.PomodoroProperties;

public interface IPomdoroPropertiesHandler
{
    Task<PomodoroSettings> GetPomodoroPropertiesAsync(Guid userId);
}

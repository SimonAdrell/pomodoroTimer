using System;
using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Web.Converter;

public static class SettingsConverter
{
    /// <summary>
    /// Converts a <see cref="UserPomodoroSettingsEntity"/> to a <see cref="PomodoroSettings"/>.
    /// </summary>
    /// <param name="source">The <see cref="UserPomodoroSettingsEntity"/> to convert.</param>
    /// <returns>A <see cref="PomodoroSettings"/> object with the converted settings.</returns>
    public static PomodoroSettings ConvertToPomodoroSettings(this UserPomodoroSettingsEntity source)
    {
        return new PomodoroSettings()
        {
            MinutesPerLongBreak = source.MinutesPerLongBreak,
            MinutesPerShortBreak = source.MinutesPerShortBreak,
            MinutesPerPomodoro = source.MinutesPerPomodoro,
            PomodorosBeforeLongBreak = source.PomodorosBeforeLongBreak
        };
    }

}

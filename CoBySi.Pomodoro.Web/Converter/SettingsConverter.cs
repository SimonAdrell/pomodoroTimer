using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Web.Converter;

internal static class SettingsConverter
{
    /// <summary>
    /// Converts a <see cref="PomodoroSettingsEntity"/> to a <see cref="PomodoroSettings"/>.
    /// </summary>
    /// <param name="source">The <see cref="PomodoroSettingsEntity"/> to convert.</param>
    /// <returns>A <see cref="PomodoroSettings"/> object with the converted settings.</returns>
    internal static PomodoroSettings? ConvertToPomodoroSettings(this PomodoroSettingsEntity? source)
    {
        if (source == null)
            return null;

        return new PomodoroSettings()
        {
            MinutesPerLongBreak = source.MinutesPerLongBreak,
            MinutesPerShortBreak = source.MinutesPerShortBreak,
            MinutesPerPomodoro = source.MinutesPerPomodoro,
            PomodorosBeforeLongBreak = source.PomodorosBeforeLongBreak
        };
    }

    internal static PomodoroSettingsEntity ConvertToUserPomodoroSettingsEntity(this PomodoroSettings source)
    {
        return new PomodoroSettingsEntity()
        {
            MinutesPerLongBreak = source.MinutesPerLongBreak,
            MinutesPerShortBreak = source.MinutesPerShortBreak,
            MinutesPerPomodoro = source.MinutesPerPomodoro,
            PomodorosBeforeLongBreak = source.PomodorosBeforeLongBreak
        };
    }

}

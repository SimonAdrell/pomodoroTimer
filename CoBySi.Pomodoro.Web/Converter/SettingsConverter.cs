using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Web.Converter;

internal static class SettingsConverter
{
    /// <summary>
    /// Converts a <see cref="UserPomodoroSettingsEntity"/> to a <see cref="PomodoroSettings"/>.
    /// </summary>
    /// <param name="source">The <see cref="UserPomodoroSettingsEntity"/> to convert.</param>
    /// <returns>A <see cref="PomodoroSettings"/> object with the converted settings.</returns>
    internal static PomodoroSettings? ConvertToPomodoroSettings(this UserPomodoroSettingsEntity? source)
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

    internal static UserPomodoroSettingsEntity ConvertToUserPomodoroSettingsEntity(this PomodoroSettings source, string userId)
    {
        return new UserPomodoroSettingsEntity()
        {
            UserID = userId,
            MinutesPerLongBreak = source.MinutesPerLongBreak,
            MinutesPerShortBreak = source.MinutesPerShortBreak,
            MinutesPerPomodoro = source.MinutesPerPomodoro,
            PomodorosBeforeLongBreak = source.PomodorosBeforeLongBreak
        };
    }

}

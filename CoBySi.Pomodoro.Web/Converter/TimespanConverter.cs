namespace CoBySi.Pomodoro.Web.Converter;

public static class TimespanConverter
{
    public static string ConvertTimeSpanToMinutesSecondsString(this TimeSpan timeSpan)
    {
        if (timeSpan.Hours > 0)
            return $"{timeSpan:hh\\:mm\\:ss}";
        return $"{timeSpan:mm\\:ss}";
    }

}

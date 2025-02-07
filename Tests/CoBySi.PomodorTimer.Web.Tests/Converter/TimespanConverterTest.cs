using CoBySi.Pomodoro.Web.Converter;

namespace CoBySi.PomodorTimer.Web.Tests.Converter;

public class TimespanConverterTest
{
    [Fact]
    public void ConvertTimeSpanToMinutesSecondsString_ShouldReturnFormattedString()
    {
        // Arrange
        TimeSpan timeSpan1 = new TimeSpan(0, 30, 45); // 1:30:45
        // Act
        string result1 = timeSpan1.ConvertTimeSpanToMinutesSecondsString();

        // Assert
        Assert.Equal("30:45", result1);
    }
    [Fact]
    public void HoursIsAdded_ConvertTimeSpanToMinutesSecondsString_ShouldReturnFormattedString()
    {
        // Arrange
        TimeSpan timeSpan1 = new TimeSpan(1, 30, 45); // 1:30:45
        // Act
        string result1 = timeSpan1.ConvertTimeSpanToMinutesSecondsString();

        // Assert
        Assert.Equal("01:30:45", result1);
    }

    [Fact]
    public void OneMinute_ConvertTimeSpanToMinutesSecondsString_ShouldReturnFormattedString()
    {
        // Arrange
        TimeSpan timeSpan1 = new TimeSpan(0, 01, 00); // 1:30:45
        // Act
        string result1 = timeSpan1.ConvertTimeSpanToMinutesSecondsString();

        // Assert
        Assert.Equal("01:00", result1);
    }
    [Fact]
    public void AlmostMinute_ConvertTimeSpanToMinutesSecondsString_ShouldReturnFormattedString()
    {
        // Arrange
        TimeSpan timeSpan1 = new TimeSpan(0, 00, 59); // 1:30:45
        // Act
        string result1 = timeSpan1.ConvertTimeSpanToMinutesSecondsString();

        // Assert
        Assert.Equal("00:59", result1);
    }
}



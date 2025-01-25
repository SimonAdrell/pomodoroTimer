using System;

namespace CoBySi.Pomodoro.Web.Settings;

public class CacheSetting
{
    public string? Namespace { get; set; }
    public long SlidingExpirationMinutes { get; set; }
}

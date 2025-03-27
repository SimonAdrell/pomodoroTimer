namespace CoBySi.Pomodoro.Repository.Models;

public record PomodoroEntity(string Id, DateTime Started, DateTime ExpectedToEnd, DateTime? Ended, double TotalNumberOfSeconds,
    string UserId, string SessionID, int PomodoroNumber) : UserBaseEntity(UserId);

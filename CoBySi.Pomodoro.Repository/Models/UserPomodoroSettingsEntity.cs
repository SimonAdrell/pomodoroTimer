using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoBySi.Pomodoro.Repository.Models;

public class UserPomodoroSettingsEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string UserID { get; set; }
    public double MinutesPerPomodoro { get; set; }
    public double MinutesPerShortBreak { get; set; }
    public double MinutesPerLongBreak { get; set; }
    public double PomodorosBeforeLongBreak { get; set; }
}

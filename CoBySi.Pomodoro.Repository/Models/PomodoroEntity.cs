using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoBySi.Pomodoro.Repository.Models;

public class PomodoroEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime Started { get; set; }
    public DateTime ExpectedToEnd { get; set; }
    public DateTime? Ended { get; set; }
    public double TotalNumberOfSeconds { get; set; }
    public string? UserId { get; set; }
    public string? SessionID { get; set; }
    public int PomodoroNumber { get; set; }
}

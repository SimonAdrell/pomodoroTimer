using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NuGet.Common;

namespace CoBySi.Pomodoro.Repository.Models;

public class NotificationEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string UserId { get; set; }
    public bool Notify { get; set; }
    public bool PlaySound { get; set; }
    public int SoundID { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastChanged { get; set; }
}

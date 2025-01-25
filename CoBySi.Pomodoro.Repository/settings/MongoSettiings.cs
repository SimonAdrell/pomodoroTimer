namespace CoBySi.Pomodoro.Repository.settings;

public class MongoSettiings
{
    public string? ConnectionString { get; set; }
    public string? Database { get; set; }
    public MongoCollection? Collections { get; set; }
}

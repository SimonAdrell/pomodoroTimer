namespace CoBySi.Pomodoro.Repository.settings;

public record PomodorosDbSettings : CosmosBaseSettings
{
    public PomodorosDbSettings(string ConnectionString, string DatatabaseName, string ContainerName, bool Setup) : base(ConnectionString, DatatabaseName, ContainerName, Setup)
    {
    }
}

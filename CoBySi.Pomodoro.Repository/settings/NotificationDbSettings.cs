namespace CoBySi.Pomodoro.Repository.settings;

public record class NotificationDbSettings : CosmosBaseSettings
{
    public NotificationDbSettings(string ConnectionString, string DatatabaseName, string ContainerName, bool Setup) : base(ConnectionString, DatatabaseName, ContainerName, Setup)
    {
    }
}

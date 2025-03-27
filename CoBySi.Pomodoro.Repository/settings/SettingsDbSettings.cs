namespace CoBySi.Pomodoro.Repository.settings;

public record class SettingsDbSettings : CosmosBaseSettings
{
    public SettingsDbSettings(string ConnectionString, string DatatabaseName, string ContainerName, bool Setup) : base(ConnectionString, DatatabaseName, ContainerName, Setup)
    {
    }
}

namespace CoBySi.Pomodoro.Repository.settings;

public record class SettingsDbSettings : CosmosBaseSettings
{
    public SettingsDbSettings(string DatatabaseName, string ContainerName, bool Setup) : base(DatatabaseName, ContainerName, Setup)
    {
    }
}

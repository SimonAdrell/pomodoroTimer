namespace CoBySi.Pomodoro.Repository.settings;

public record CosmosUserSetting : CosmosBaseSettings
{
    public CosmosUserSetting(string DatabaseName, string ContainerName, bool Setup) : base(DatabaseName, ContainerName, Setup)
    {
    }
}
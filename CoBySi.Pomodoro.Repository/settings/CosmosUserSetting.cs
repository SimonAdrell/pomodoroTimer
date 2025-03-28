namespace CoBySi.Pomodoro.Repository.settings;
public record CosmosUserSetting : CosmosBaseSettings
{
    public CosmosUserSetting(string ConnectionString, string DatatabaseName, string ContainerName, bool Setup) : base(ConnectionString, DatatabaseName, ContainerName, Setup)
    {
    }
}
namespace CoBySi.Pomodoro.Repository.settings;

public abstract record CosmosBaseSettings(string DatabaseName, string ContainerName, bool Setup);
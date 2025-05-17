using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Extensions.Configuration;

namespace CoBySi.Pomodoro.Repository.Repositories;

public class SettingsRepository : CosmosRepositoryBase<UserSettings>, ISettingsRepository
{
    public SettingsRepository(SettingsDbSettings settings, IConfiguration configuration) : base(settings, configuration)
    {
    }

    public async Task<UserSettings?> GetUserSettingsByUserId(string userId, CancellationToken cancellationToken)
    {
        return await GetFirstOrDefaultByUserId(userId, cancellationToken);
    }

    public async Task<UserSettings?> UpsertUserSetting(UserSettings userSettings, CancellationToken cancellationToken)
    {
        return await Upsert(userSettings, cancellationToken);
    }
}

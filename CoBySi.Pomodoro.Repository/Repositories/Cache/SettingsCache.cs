using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace CoBySi.Pomodoro.Repository.Repositories.Cache;

public class SettingsCache : CacheBase<UserSettings>, ISettingsCache
{
    public SettingsCache(IDistributedCache distributedCache, IOptions<RedisSettings> cacheSetting) : base(distributedCache, cacheSetting?.Value.SettingsCache)
    {
    }
}

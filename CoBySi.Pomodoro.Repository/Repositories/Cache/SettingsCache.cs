using CoBySi.Pomodoro.Repository.Models;
using CoBySi.Pomodoro.Repository.Repositories.Cache;
using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace CoBySi.Pomodoro.Web.Cache;

public class SettingsCache : CacheBase<UserSettings>, ISettingsCache
{
    public SettingsCache(IDistributedCache distributedCache, IOptions<RedisSettings> cacheSetting) : base(distributedCache, cacheSetting?.Value.SettingsCache)
    {
    }
}

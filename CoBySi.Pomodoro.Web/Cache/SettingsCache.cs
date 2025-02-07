using CoBySi.Pomodoro.Web.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace CoBySi.Pomodoro.Web.Cache;

public class SettingsCache : CacheBase, ISettingsCache
{
    public SettingsCache(IDistributedCache distributedCache, IOptions<RedisSettings> cacheSetting) : base(distributedCache, cacheSetting?.Value.SettingsCache)
    {
    }
}

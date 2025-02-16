using CoBySi.Pomodoro.Web.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace CoBySi.Pomodoro.Web.Cache;

public class TimerCache : CacheBase, ITimerCache
{
    public TimerCache(IDistributedCache distributedCache, IOptions<RedisSettings> cacheSetting) : base(distributedCache, cacheSetting?.Value.TimerCache)
    {
    }
}

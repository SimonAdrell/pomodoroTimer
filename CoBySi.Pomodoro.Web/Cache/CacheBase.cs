using System;
using CoBySi.Pomodoro.Web.Settings;
using Microsoft.Extensions.Caching.Distributed;

namespace CoBySi.Pomodoro.Web.Properties;

public abstract class CacheBase
{
    private readonly IDistributedCache _distributedCache;
    private readonly CacheSetting _cacheSetting;

    protected CacheBase(IDistributedCache distributedCache, CacheSetting cacheSetting)
    {
        ArgumentNullException.ThrowIfNull(cacheSetting, nameof(cacheSetting));
        _distributedCache = distributedCache;
        _cacheSetting = cacheSetting;
    }

    private string GetKey<T>(string key)
    {
        return $"{_cacheSetting.Namespace}:{typeof(T).Name}:{key}";
    }

    public T? Get<T>(string key)
    {
        var value = _distributedCache.GetString(GetKey<T>(key));
        return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
    }
    public void Set<T>(string key, T value)
    {
        var options = new DistributedCacheEntryOptions();
        options.SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSetting.SlidingExpirationMinutes));
        _distributedCache.SetString(GetKey<T>(key), System.Text.Json.JsonSerializer.Serialize(value), options);
    }
}

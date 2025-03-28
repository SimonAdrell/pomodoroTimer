using CoBySi.Pomodoro.Repository.settings;
using Microsoft.Extensions.Caching.Distributed;

namespace CoBySi.Pomodoro.Repository.Repositories.Cache;

public abstract class CacheBase<T> : ICacheBase<T>
{
    private readonly IDistributedCache _distributedCache;
    private readonly CacheSetting _cacheSetting;

    protected CacheBase(IDistributedCache distributedCache, CacheSetting? cacheSetting)
    {
        ArgumentNullException.ThrowIfNull(cacheSetting, nameof(cacheSetting));
        _distributedCache = distributedCache;
        _cacheSetting = cacheSetting;
    }

    private string GetKey(string key)
    {
        return $"{_cacheSetting.Namespace}:{typeof(T).Name}:{key}";
    }

    public async Task<T?> GetAsync(string key, CancellationToken cancellationToken)
    {
        var value = await _distributedCache.GetStringAsync(GetKey(key), cancellationToken);
        return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
    }
    public async Task SetAsync(string key, T value, CancellationToken cancellationToken)
    {
        var options = new DistributedCacheEntryOptions();
        options.SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSetting.SlidingExpirationMinutes));
        await _distributedCache.SetStringAsync(GetKey(key), System.Text.Json.JsonSerializer.Serialize(value), options, cancellationToken);
    }
}

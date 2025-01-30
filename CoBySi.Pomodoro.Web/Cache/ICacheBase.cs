namespace CoBySi.Pomodoro.Web.Cache;

public interface ICacheBase
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken);
}

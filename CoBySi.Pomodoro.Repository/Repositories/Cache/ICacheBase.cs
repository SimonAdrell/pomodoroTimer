namespace CoBySi.Pomodoro.Repository.Repositories.Cache;

public interface ICacheBase<T>
{
    public Task<T?> GetAsync(string key, CancellationToken cancellationToken);
    public Task SetAsync(string key, T value, CancellationToken cancellationToken);
}

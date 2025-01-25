namespace CoBySi.Pomodoro.Web.Cache;

public interface ICacheBase
{
    public T? Get<T>(string key);
    public void Set<T>(string key, T value);
}

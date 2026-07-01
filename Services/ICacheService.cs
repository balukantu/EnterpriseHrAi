namespace HrAi.Api.Services;

public interface ICacheService
{
    bool TryGet<T>(string key, out T? value);
    void Set<T>(string key, T value, TimeSpan expiration);
    void Remove(string key);
}
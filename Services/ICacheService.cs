namespace NSubstituteTest.Services;

public interface ICacheService
{
    Task<T?> GetOrSetAsync<T>(string key, Func<CancellationToken,Task<T>> factory, TimeSpan? timeExpiration = null, CancellationToken ct = default);
}

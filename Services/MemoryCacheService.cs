
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace NSubstituteTest.Services;

[ExcludeFromCodeCoverage]
public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    public async Task<T?> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? timeExpiration = null, CancellationToken ct = default)
    {
        var result = await cache.GetOrCreateAsync(key, async entry =>
        {
            if (timeExpiration.HasValue)
            {
                entry.AbsoluteExpirationRelativeToNow = timeExpiration;
            }
            return await factory(ct);
        });

        return result;
    }
}

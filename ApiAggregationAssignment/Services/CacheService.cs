using ApiAggregationAssignment.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace ApiAggregationAssignment.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T?> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> fetchFromService, TimeSpan expiration)
        {

            if (_memoryCache.TryGetValue(cacheKey, out T? cachedValue))
            {
                return cachedValue;
            }

            
            var result = await fetchFromService();

            cachedValue = await fetchFromService();
            _memoryCache.Set(cacheKey, cachedValue, expiration);
            return cachedValue;
        }
    }
}

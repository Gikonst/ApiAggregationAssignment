using ApiAggregationAssignment.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregationAssignment.Tests
{
    public class CacheServiceTests
    {
        [Fact]
        public async Task GetOrCreateAsync_ShouldReturnCachedValue()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);
            var cacheKey = "test_key";
            var cachedValue = "cached_value";

            // Add value to cache
            memoryCache.Set(cacheKey, cachedValue, TimeSpan.FromMinutes(10));

            // Act
            var result = await cacheService.GetOrCreateAsync<string>(cacheKey, () => Task.FromResult("new_value"), TimeSpan.FromMinutes(10));

            // Assert
            Assert.Equal(cachedValue, result); // Should return the cached value
        }
    }
}

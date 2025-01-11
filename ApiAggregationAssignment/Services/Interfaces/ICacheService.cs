using System.Threading.Tasks;

namespace ApiAggregationAssignment.Services.Interfaces
{
    public interface ICacheService
    {
        public Task<T?> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan expiration);
    }
}

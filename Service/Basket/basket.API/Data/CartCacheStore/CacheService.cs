using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace basket.API.Data.CartCacheStore
{
    public class CacheService<T>(IDistributedCache cache) : ICacheService<T> where T : class
    {
        private DateTimeOffset DefaultAbsoluteTime => DateTimeOffset.UtcNow.AddMinutes(5);
        private TimeSpan DefaultSlidingTime => TimeSpan.FromMinutes(2);

        public async Task<T?> Get(string key)
        {
            var cacheResponse = await cache.GetAsync(key);
            if (cacheResponse == null)
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(cacheResponse));
        }

        public async Task Set(string key, T obj, DateTimeOffset? absoluteTime = null, TimeSpan? slidingTime = null)
        {
            await Remove(key);
            var Bcart = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
            await cache.SetAsync(key, Bcart, new DistributedCacheEntryOptions() 
            {
                AbsoluteExpiration = absoluteTime ?? DefaultAbsoluteTime,
                SlidingExpiration = slidingTime ?? DefaultSlidingTime,
            });
        }

        public async Task Remove(string key)
        {
            await cache.RemoveAsync(key);
        }
    }
}
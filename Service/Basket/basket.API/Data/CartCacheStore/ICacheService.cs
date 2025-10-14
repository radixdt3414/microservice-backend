namespace basket.API.Data.CartCacheStore
{
    public interface ICacheService<T>
    {
        Task<T?> Get(string key);
        Task Set(string key, T obj, DateTimeOffset? absoluteTime = null, TimeSpan? slidingTime = null);
        Task Remove(string key);
    }
}

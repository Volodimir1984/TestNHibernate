using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace CompaniesService
{
    public static class CacheExtension
    {
        //Returns cache data with redis
        public static async Task<T> GetCacheByKeyAsync<T>(this IDistributedCache cache, string key) where T: class
        {
            var result = await cache.GetStringAsync(key);

            return string.IsNullOrEmpty(result) ? null : JsonSerializer.Deserialize<T>(result);
        }

        //Writes data in Redis
        public static async Task SetCacheAsync<T>(this IDistributedCache cache, string key, T value) where T : class
        {
            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            };

            var result = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, result, cacheEntryOptions);
        }
    }
}

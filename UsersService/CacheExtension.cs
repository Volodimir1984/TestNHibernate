using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace UsersService
{
    public static class CacheExtension
    {
        public static async Task<T> GetCacheByKeyAsync<T>(this IDistributedCache cache, string key) where T : class
        {
            var result = await cache.GetStringAsync(key);

            return string.IsNullOrEmpty(result) ? null : JsonSerializer.Deserialize<T>(result);
        }

        public static async Task SetCacheAsync<T>(this IDistributedCache cache, string key, T value) where T : class
        {
            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60),
                SlidingExpiration = TimeSpan.FromSeconds(30)
            };

            var result = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, result, cacheEntryOptions);
        }
    }
}

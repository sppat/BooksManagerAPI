using BooksManagerAPI.Interfaces.CacheInterfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BooksManagerAPI.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string key, object response, TimeSpan liveTime)
        {
            if (response is null) return;

            var serializedResponse = JsonConvert.SerializeObject(response);

            await _distributedCache.SetStringAsync(key, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = liveTime,
            });
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            return await _distributedCache.GetStringAsync(key);
        }

        public async Task RemoveCachedAsync(string key) => await _distributedCache.RemoveAsync(key);
    }
}

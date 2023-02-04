using FreelanceStormer.Models;
using FreelanceStormer.Utils.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FreelanceStormer.Utils
{
    public class DataMemoryCache : IDataCache
    {
        private readonly IMemoryCache _memoryCache;

        public DataMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? Get<T>(int key) where T : class
        {
            _memoryCache.TryGetValue(key, out T? data);

            return data;
        }

        public void Set<T>(int key, T data)
        {
            _memoryCache.Set(key, data);
            return;
        }
    }
}

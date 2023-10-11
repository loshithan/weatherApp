using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Threading.Tasks;

namespace weatherApp.DAL
{
    public class CacheServices
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        
        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public void Set<T>(string key, T value, DateTimeOffset cacheExpiry)
        {
            Cache.Add(key, value, cacheExpiry);
        }

    }
}
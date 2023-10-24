using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weatherApp.CacheProvider
{
    public class CacheService : ICacheService
    {
        public object Get(string key)
        {
            return HttpContext.Current.Cache[key];
        }

        public void Insert(string key, object value, DateTime absoluteExpiration)
        {
            HttpContext.Current.Cache.Insert(key, value, null, absoluteExpiration, TimeSpan.Zero);
        }

        public void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
    }
}
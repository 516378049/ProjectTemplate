using WebAPI.Core.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkDev.Caching;
using ThinkDev.Caching.Caches;
namespace WebAPI.Core
{
    public class CacheManager
    {
        public static IRedis RedisCache;
        public static ICache RuntimeCache;
        public static ICache DefaultCache;

        static CacheManager()
        {
            RedisCache = (RedisCache)(CacheFactory.GetCache("RedisDefault"));
            RuntimeCache = CacheFactory.GetCache("runtime");
            DefaultCache = CacheFactory.GetCache("RedisDefault");
        }

        public static string GetKey(string key)
        {
            return string.Concat(ConfigHelper.WebAPICacheKeyPre, ".", key);
        }
    }
}

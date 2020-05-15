using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkDev.Caching;
using ThinkDev.Caching.Caches;

namespace Framework
{
    public class CacheManager
    {
        /// <summary>
        /// 默认Redis缓存处理器
        /// </summary>
        public static IRedis RedisCache = (RedisCache)(CacheFactory.GetCache("redis"));

        /// <summary>
        /// 运行时缓存
        /// </summary>
        public static IRunTime RunTimeCache = CacheFactory.GetRunTime("runtime");

        public static string GetKey(string key)
        {
            return string.Concat(ConfigHelper.CacheKeyPre, ".", key);
        }

        public static string GetSSOKey(string key)
        {
            return string.Concat(ConfigHelper.CacheSSOKeyPre, ".", key);
        }
    }
}

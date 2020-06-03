using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wxPayApiMVC.lib
{

    public class CacheHelper
    {
        //取不存在cacheKey会返回null
        //重复insert cache 会替换之前的值和设置
        //过期时间 TimeSpan对象可以试时分秒等

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        //public static void SetCache(string CacheKey, object objObject)
        //{
        //    System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        //    objCache.Insert(CacheKey, objObject);
        //}
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        //public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        //{
        //    System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        //    objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        //}
        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveAllCache(string CacheKey)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(CacheKey);
        }
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeBlock.Controllers
{
    public class RedisController : Controller
    {
        // GET: Redis
        //first step: ref StackExchange.Redis from nuget
        //second step: ref ThinkDev.Caching.dll 
        //third step: config ThinkDev.Caching.config file
        public ActionResult Index()
        {
            SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
            dic.Add("a", "b");
            CacheManager.RedisCache.Set<SortedDictionary<string, object>>("RidisValue", dic, 60 * 12);
            SortedDictionary<string, object> RidisValue = CacheManager.RedisCache.Get<SortedDictionary<string, object>>("RidisValue");
            return View();
        }
    }

    
}
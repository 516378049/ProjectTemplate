using WebAPI.Core.DataAccess.App;
using WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Business.App
{
    public class AppRule : Base
    {
        #region 缓存Key常量
        private const string CacheKey_AppHash = "WebAPI.Core.AppHash";
        private const string CacheKey_ApiHash = "WebAPI.Core.ApiHash";
        private const string CacheKey_AppApiRelationHash = "WebAPI.Core.AppApiRelationHash";
        #endregion

        static object AppInfoLockObj = new object();
        static object ApiInfoLockObj = new object();
        static object RelationLockObj = new object();
        const int RunTimeCacheMins = 1;
        AppDA da = new AppDA();
        static AppDA staticDA = new AppDA();


        /// <summary>
        /// 根据AppID获取应用信息
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public AppInfo GetSingleAppInfo(int appId)
        {
            string key = "AppInfo::AppID::" + appId.ToString();
            key = CacheManager.GetKey(key);
            AppInfo app = CacheManager.RuntimeCache.Get<AppInfo>(key);
            if (app == null)
            {
                lock (AppInfoLockObj)
                {
                    app = CacheManager.RuntimeCache.Get<AppInfo>(key);
                    if (app == null)
                    {
                        app = CacheManager.RedisCache.HGet<AppInfo>(CacheManager.GetKey(CacheKey_AppHash), appId.ToString());
                        if (app != null)
                        {
                            CacheManager.RuntimeCache.Set<AppInfo>(key, app, RunTimeCacheMins);
                        }
                    }
                }
            }
            return app;
        }

        /// <summary>
        /// 根据Api名称获取详细信息
        /// </summary>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public ApiInfo GetSingleApiInfo(string apiModule, string apiName)
        {
            string key = "ApiInfo::" + apiModule.ToLower() + "." + apiName.ToLower();
            key = CacheManager.GetKey(key);
            ApiInfo api = CacheManager.RuntimeCache.Get<ApiInfo>(key);
            if (api == null)
            {
                lock (ApiInfoLockObj)
                {
                    api = CacheManager.RuntimeCache.Get<ApiInfo>(key);
                    if (api == null)
                    {
                        api = CacheManager.RedisCache.HGet<ApiInfo>(CacheManager.GetKey(CacheKey_ApiHash), apiModule.ToLower() + "." + apiName.ToLower());
                        if (api != null)
                        {
                            CacheManager.RuntimeCache.Set<ApiInfo>(key, api, RunTimeCacheMins);
                        }
                    }
                }
            }
            return api;
        }

        /// <summary>
        /// 获取应用与Api的关系对象
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="apiID"></param>
        /// <returns></returns>
        public App_ApiRelation GetRelation(int appID, int apiID)
        {
            string key = "AppApiRelation::" + appID.ToString() + "." + apiID.ToString();
            key = CacheManager.GetKey(key);
            App_ApiRelation relation = CacheManager.RuntimeCache.Get<App_ApiRelation>(key);
            if (relation == null)
            {
                lock (RelationLockObj)
                {
                    relation = CacheManager.RuntimeCache.Get<App_ApiRelation>(key);
                    if (relation == null)
                    {
                        relation = CacheManager.RedisCache.HGet<App_ApiRelation>(CacheManager.GetKey(CacheKey_AppApiRelationHash), appID.ToString() + "." + apiID.ToString());
                        if (relation != null)
                        {
                            CacheManager.RuntimeCache.Set<App_ApiRelation>(key, relation, RunTimeCacheMins);
                        }
                    }
                }
            }
            return relation;
        }

        /// <summary>
        /// 全局统一将App、Api相关数据置入缓存
        /// </summary>
        public static void GlobalInitAppCache()
        {
            #region 置入AppInfo缓存

            List<AppInfo> list_AppInfo = staticDA.SelectList<AppInfo>();
            foreach (AppInfo app in list_AppInfo)
            {
                CacheManager.RedisCache.HSet<AppInfo>(CacheManager.GetKey(CacheKey_AppHash), app.AppID.ToString(), app);
            }
            #endregion

            #region 置入ApiInfo缓存

            List<ApiInfo> list_ApiInfo = staticDA.SelectList<ApiInfo>();
            foreach (ApiInfo api in list_ApiInfo)
            {
                CacheManager.RedisCache.HSet<ApiInfo>(CacheManager.GetKey(CacheKey_ApiHash), api.Module.ToLower() + "." + api.ApiName.ToLower(), api);
            }
            #endregion

            #region 置入App-ApiRelation缓存

            List<App_ApiRelation> list_appApiRelation = staticDA.SelectList<App_ApiRelation>();
            foreach (App_ApiRelation relation in list_appApiRelation)
            {
                CacheManager.RedisCache.HSet<App_ApiRelation>(CacheManager.GetKey(CacheKey_AppApiRelationHash), relation.AppID.ToString() + "." + relation.ApiID.ToString(), relation);
            }
            #endregion
        }

        /// <summary>
        /// 向缓存置入App对象
        /// </summary>
        /// <param name="app"></param>
        public static void Cache_SetApp(AppInfo app)
        {
            if (app != null)
            {
                CacheManager.RedisCache.HSet<AppInfo>(CacheManager.GetKey(CacheKey_AppHash), app.AppID.ToString(), app);
            }
        }

        /// <summary>
        /// 向缓存移除App对象
        /// </summary>
        /// <param name="app"></param>
        public static void Cache_RemoveApp(int appID)
        {
            CacheManager.RedisCache.HDel(CacheManager.GetKey(CacheKey_AppHash), appID.ToString());
        }

        /// <summary>
        /// 向缓存置入Api对象
        /// </summary>
        /// <param name="app"></param>
        public static void Cache_SetApi(ApiInfo api)
        {
            if (api != null)
            {
                CacheManager.RedisCache.HSet<ApiInfo>(CacheManager.GetKey(CacheKey_ApiHash), api.Module.ToLower() + "." + api.ApiName.ToLower(), api);
            }
        }

        /// <summary>
        /// 向缓存移除Api对象
        /// </summary>
        /// <param name="app"></param>
        public static void Cache_RemoveApi(string apiModule, string apiName)
        {
            if (!string.IsNullOrEmpty(apiModule) && !string.IsNullOrEmpty(apiName))
            {
                CacheManager.RedisCache.HDel(CacheManager.GetKey(CacheKey_ApiHash), apiModule.ToLower() + "." + apiName.ToLower());
            }
        }

        /// <summary>
        /// 向缓存置入App_ApiRelation
        /// </summary>
        /// <param name="app"></param>
        public static void Cache_SetApp_ApiRelation(App_ApiRelation relation)
        {
            if (relation != null)
            {
                CacheManager.RedisCache.HSet<App_ApiRelation>(CacheManager.GetKey(CacheKey_AppApiRelationHash), relation.AppID.ToString() + "." + relation.ApiID.ToString(), relation);
            }
        }

        /// <summary>
        /// 从缓存移除App_ApiRelation
        /// </summary>
        /// <param name="app"></param>
        public static void Cache_RemoveApp_ApiRelation(int appID, int apiID)
        {
            CacheManager.RedisCache.HDel(CacheManager.GetKey(CacheKey_AppApiRelationHash), appID.ToString() + "." + apiID.ToString());
        }



        /// <summary>
        /// 为某Api添加调用次数
        /// </summary>
        /// <param name="apiID"></param>
        /// <param name="apiName"></param>
        /// <param name="count"></param>
        public static void ApiAddCount(string appID, string apiId, string apiModule, string apiName, string retCode, int count = 1)
        {
            CountManager.ApiCounter.AddCount(apiModule + "," + apiName + "," + appID + "," + apiId + "," + retCode, count);
        }

        /// <summary>
        /// 将某Api累积调用次数入库
        /// </summary>
        /// <param name="apiInfo"></param>
        /// <param name="count"></param>
        public static bool UpdateApiCountInfo(string apiInfo, int count)
        {
            LogManager.DefaultLogger.Info("UpdateApiCountInfo=>" + apiInfo + ":" + count.ToString());
            string[] apiInfos = apiInfo.Split(',');
            if (apiInfos.Count() >= 5)
            {
                ApiCountInfo info = new ApiCountInfo() { Module = apiInfos[0], ApiName = apiInfos[1], AppID = apiInfos[2], ApiID = apiInfos[3], RetCode = apiInfos[4], Count = count, CountTime = DateTime.Parse(DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:00")) };
                staticDA.Insert<ApiCountInfo>(info, a => a.Module, a => a.ApiName, a => a.AppID, a => a.ApiID, a => a.RetCode, a => a.Count, a => a.CountTime);
                return true;
            }
            return true;
        }
    }
}

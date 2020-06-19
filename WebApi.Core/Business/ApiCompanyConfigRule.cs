using WebAPI.Core.DataAccess;
using WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Business
{
    public class ApiCompanyConfigRule : Base
    {
        private ApiCompanyConfigDA da = new ApiCompanyConfigDA();
        private object lockObj = new object();

        #region 根据AppID查询相关配置信息
        /// <summary>
        /// 根据AppID查询相关配置信息
        /// </summary>
        public List<ApiCompanyConfigInfo> ApiCompanyConfigInfo_QueryList(int appid)
        {
            string key = CacheManager.GetKey(string.Concat("ApiCompanyConfigInfoQueryList.", appid));
            List<ApiCompanyConfigInfo> lstApiCompanyConfigInfo = CacheManager.RedisCache.Get<List<ApiCompanyConfigInfo>>(key);
            if (lstApiCompanyConfigInfo == null || lstApiCompanyConfigInfo.Count == 0)
            {
                lock (lockObj)
                {
                    lstApiCompanyConfigInfo = CacheManager.RedisCache.Get<List<ApiCompanyConfigInfo>>(key);
                    if (lstApiCompanyConfigInfo == null || lstApiCompanyConfigInfo.Count == 0)
                    {
                        lstApiCompanyConfigInfo = this.da.ApiCompanyConfigInfo_QueryList(appid);
                        if (lstApiCompanyConfigInfo != null)
                        {
                            CacheManager.RedisCache.Set<List<ApiCompanyConfigInfo>>(key, lstApiCompanyConfigInfo, 60);
                        }
                    }
                }
            }
            return lstApiCompanyConfigInfo;
        }
    }
    #endregion
}

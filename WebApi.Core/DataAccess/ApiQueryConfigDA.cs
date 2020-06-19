using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WebAPI.Core.Models;
using ThinkDev.Data;
using ThinkDev.Data.MSSql;

namespace WebAPI.Core.DataAccess
{
    public class ApiQueryConfigDA : DBContextBase
    {
        #region 根据AppID查询相关的Query配置信息
        /// <summary>
        /// 根据AppID查询相关的Query配置信息
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public ApiQueryConfigInfo ApiQueryConfigInfo_Query(int appid)
        {
            string sql = SqlBuilder.From(GetTableName(typeof(ApiQueryConfigInfo)))
                .Where("AppID", MatchType.Equal, appid)
                .And("DelFlag", MatchType.Equal, 0)
                .Select()
                .GetSql();
            using (IDataReader dr = ExecuteDataReader(sql))
            {
                return GetObject<ApiQueryConfigInfo>(dr);
            }
        }
        #endregion 根据AppID查询相关的Query配置信息
    }
}

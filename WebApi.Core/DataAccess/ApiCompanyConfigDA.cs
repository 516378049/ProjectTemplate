using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPI.Core.Models;
using ThinkDev.Data.MSSql;
using ThinkDev.Data;
using System.Data;

namespace WebAPI.Core.DataAccess
{
    public class ApiCompanyConfigDA : DBContextBase
    {
        public ApiCompanyConfigDA()
        {
            //特别的，如果当前类数据库连接字符串为非Default，则可以在这里重置CurrentConnection属性
            //ChangeConnection(ConnectionManager.WebApi);
        }

        #region 根据AppID查询相关配置信息
        /// <summary>
        /// 根据AppID查询相关配置信息
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public List<ApiCompanyConfigInfo> ApiCompanyConfigInfo_QueryList(int appid)
        {
            string sql = SqlBuilder.From(GetTableName(typeof(ApiCompanyConfigInfo)))
                .Where("AppID", MatchType.Equal, appid)
                .And("DelFlag", MatchType.Equal, 0)
                .Select()
                .OrderBy("SectionID", SortType.Asc)
                .GetSql();
            using (IDataReader dr = ExecuteDataReader(sql))
            {
                return GetObjectList<ApiCompanyConfigInfo>(dr);
            }
        }
        #endregion
    }
}

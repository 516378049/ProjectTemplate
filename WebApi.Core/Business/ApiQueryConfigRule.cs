using WebAPI.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WebAPI.Core.Models;
using ThinkDev.Data;
using ThinkDev.Data.MSSql;

namespace WebAPI.Core.Business
{
    public class ApiQueryConfigRule : Base
    {
        private ApiQueryConfigDA da = new ApiQueryConfigDA();

        #region 根据AppID查询相关的Query配置信息
        /// <summary>
        /// 根据AppID查询相关的Query配置信息
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public ApiQueryConfigInfo ApiQueryConfigInfo_Query(int appid)
        {
            return da.ApiQueryConfigInfo_Query(appid);
        }
        #endregion 根据AppID查询相关的Query配置信息

    }
}

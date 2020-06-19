using WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkDev.Data;
namespace WebAPI.Core.DataAccess
{
    public class ModelTableMapping
    {
        public static void InitMapping()
        {
            TableModelMapper.AddMapping(typeof(AppInfo), "[dbo].[App_Info]");
            TableModelMapper.AddMapping(typeof(ApiInfo), "dbo.Api_Info");
            TableModelMapper.AddMapping(typeof(ApiCountInfo), "dbo.Api_CountLog");
            TableModelMapper.AddMapping(typeof(App_ApiRelation), "dbo.App_ApiRelation");
            TableModelMapper.AddMapping(typeof(ApiCompanyConfigInfo), "dbo.ApiCompanyConfigInfo");
            TableModelMapper.AddMapping(typeof(ApiQueryConfigInfo), "dbo.ApiQueryConfigInfo");

        }

        /// <summary>
        /// 修增Map规则
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tableName"></param>
        public static void AddMapping(Type t, string tableName)
        {
            TableModelMapper.AddMapping(t, tableName);
        }
    }
}

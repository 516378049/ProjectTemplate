using WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.DataAccess.App
{
    public class AppDA : DBContextBase
    {
        public AppDA()
        {
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="PageSize"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public List<AppInfo> GetDemoWithSqlPaged(int Start, int PageSize, AppInfo app, out int TotalCount)
        {
            TotalCount = 0;
            //1.查询总数量
            TotalCount = From<AppInfo>().Count();

            //2.具体查询条件
            List<AppInfo> demo = From<AppInfo>()
                .Skip(Start)
                .Take(PageSize)
                .OrderBy(a => a.AppID)
                .SelectList();

            return demo;
        }

    }
}

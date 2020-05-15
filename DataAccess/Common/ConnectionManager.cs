/******************************************************************
 * 功能描述：所有数据库连接字符串的定义
 * 创建时间：2015-03-04
 * 作者：changchun
 * 版本：1.0
 * 修订描述：
 * 最后修订日期：2020-1-13
 ******************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.NewTenancy.DataAccess
{
    public static class ConnectionManager
    {
        /// <summary>
        /// SPS 数据库连接
        /// </summary>
        public static string NEW_TENANCY { get { return ConfigurationManager.ConnectionStrings["JR_NEW_TENANCY"].ConnectionString; } }
    }
}

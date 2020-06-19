using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WebAPI.Core.DataAccess
{
    public class ConnectionManager
    {
        /// <summary>
        /// Default主库数据库连接
        /// </summary>
        public static string Default { get { return ConfigurationManager.ConnectionStrings["Default"].ConnectionString; } }

        /// <summary>
        /// B2B数据库连接
        /// </summary>
        public static string B2B { get { return ConfigurationManager.ConnectionStrings["B2B"].ConnectionString; } }

        /// <summary>
        /// WebApi数据库连接
        /// </summary>
        public static string WebApi { get { return ConfigurationManager.ConnectionStrings["WebApi"].ConnectionString; } }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        public static string Test { get { return ConfigurationManager.ConnectionStrings["Test"].ConnectionString; } }
    }
}

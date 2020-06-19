using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ThinkDev.Logging;
namespace WebAPI.Core
{
    /// <summary>
    /// 项目日志帮助类，本类提供各类日志对象访问入口
    /// </summary>
    internal class LogManager
    {
        /// <summary>
        /// 项目默认日志对象
        /// </summary>
        public static Logger DefaultLogger
        {
            get
            {
                return LoggerFactory.GetLogger("DefaultLogger");
            }
        }

        public static Logger WebApiLogger
        {
            get
            {
                return LoggerFactory.GetLogger("WebApiLogger");
            }
        }
    }
}

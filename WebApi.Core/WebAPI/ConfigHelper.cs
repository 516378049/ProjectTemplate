using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WebAPI.Core.WebAPI
{
    internal class ConfigHelper
    {
        private static int MonitorLimitPageTime = 0;

        /// <summary>
        /// 获取监控时长阀值
        /// </summary>
        /// <returns></returns>
        public static int GetMonitorLimitPageTime()
        {
            if (MonitorLimitPageTime == 0)
            {
                if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Runda.Monitor.Client.LimitPageTimeSpan"], out MonitorLimitPageTime))
                {
                    MonitorLimitPageTime = 1000;
                }
            }
            return MonitorLimitPageTime;
        }

        public static string WebAPICacheKeyPre { get { return ConfigurationManager.AppSettings["WebAPICacheKeyPre"]; } }
    }
}

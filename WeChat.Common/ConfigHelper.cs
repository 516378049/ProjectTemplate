using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Common
{
    public static class CloudConfigHelper
    {

        public static string GetSetting(string key)
        {
            String ret = CloudConfigurationManager.GetSetting(key);
            if (!String.IsNullOrWhiteSpace(ret))
                return ret;

            ret = System.Configuration.ConfigurationManager.AppSettings[key];
            
            return ret;
        }
    }
}

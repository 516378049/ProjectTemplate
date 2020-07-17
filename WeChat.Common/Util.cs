using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WeChat.Common
{
    public class Util
    {
        public static long CreateTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static string UploadImage(byte[] bytes, string suffix)
        {
            var endPoint = CloudConfigHelper.GetSetting("PicRootPath") ;
            var StorageConn = CloudConfigHelper.GetSetting("StorageAccount");
            AzureBlob blob = new AzureBlob(StorageConn, "editors", endPoint);
            var serverUrl = blob.SaveFile(Guid.NewGuid().ToString() + suffix, bytes);
            return serverUrl;
        }

        public static DateTime UtcToChinaTime(DateTime utcTime)
        {
            TimeZoneInfo chinaTimeZone = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(p => p.Id == "China Standard Time");
            if (chinaTimeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(utcTime, chinaTimeZone);
            }
            else
            {
                return utcTime;
            }
        }

        public static string UtcToChinaTime(object utcTimeObject, string format = "yyyy/MM/dd HH:mm:ss")
        {
            if (utcTimeObject == null) return null;
            DateTime utcTime;
            if (DateTime.TryParse(utcTimeObject.ToString(), out utcTime))
            {
                return UtcToChinaTime(utcTime).ToString(format);
            }

            return null;
        }

        public static string UtcToClientTime(object utcTimeObject, int utcOffset, string format = "yyyy/MM/dd HH:mm:ss")
        {
            if (utcTimeObject == null) return "";
            DateTime utcTime;
            if (DateTime.TryParse(utcTimeObject.ToString(), out utcTime))
            {
                return utcTime.AddHours(utcOffset).ToString(format);
            }
            return "";
        }
        public static string AppFullPath
        {
            get { return GetFullUrl("~"); }
        }
        public static string GetFullUrl(string path = "~")
        {
            if (HttpContext.Current == null)
            {
                return path;
            }

            var request = HttpContext.Current.Request;

            if (VirtualPathUtility.IsAbsolute(path))
            {
                return
                request.Url.Scheme + "://"
                + request.Url.Authority
                + request.ApplicationPath
                + path;
            }
            else
            {
                return
                request.Url.Scheme + "://"
                + request.Url.Authority
                + VirtualPathUtility.ToAbsolute(path);
            }
        }
    }

    public class ServiceBusQueueHelper
    {
        private static QueueClient messagClient = null;

        public static QueueClient Instance()
        {
            if (messagClient == null)
            {
                string connectionString = WeChat.Common.CloudConfigHelper.GetSetting("Microsoft.WX.Callback");
                messagClient = QueueClient.CreateFromConnectionString(connectionString, "wxo");
            }
            return messagClient;
        }
    }
}

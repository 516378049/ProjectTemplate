using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

using WebAPI.Core;
using WebAPI.Core.Framework;
namespace WebAPI.Core.WebAPI
{
    public class WebFuncHelper
    {
        /// <summary>
        /// 返回带有WebApi层标示的错误码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetErrCode(string code)
        {
            return CoreConstDefine.ErrCode_Pre_WebAPI + code;
        }

        /// <summary>
        /// 从Head获取某对应Key的值，如果不存在，返回空字符串
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHeadValue(HttpRequestMessage request, string key)
        {
            if (request.Headers.Contains(key))
            {
                return request.Headers.GetValues(key).FirstOrDefault();
            }
            return string.Empty;
        }

        /// <summary>
        /// 将Key与value置入HttpHead
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        public static void WriteHeadValue(HttpRequestMessage request, string key, string value)
        {
            if (request.Headers.Contains(key))
            {
                request.Headers.Remove(key);
            }
            request.Headers.Add(key, value);
        }

        /// <summary>
        /// 从Request获取ApiName
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetApiName(HttpRequestMessage request)
        {
            if (request != null)
            {
                string apiName = "";
                string[] apiPaths = request.RequestUri.AbsolutePath.Split('/');
                if (apiPaths.Length > 0)
                {
                    apiName = apiPaths[apiPaths.Length - 1];
                }
                if (!string.IsNullOrEmpty(apiName))
                {
                    return apiName = apiName.ToLower();
                }
                return apiName;
            }
            return "";
        }

        /// <summary>
        /// 按规则获取当前API所在模块
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetApiModule(HttpRequestMessage request)
        {
            if (request != null)
            {
                string apiModule = "";
                string[] apiPaths = request.RequestUri.AbsolutePath.Split('/');
                if (apiPaths.Length > 2)
                {
                    apiModule = apiPaths[1];
                }
                return apiModule;
            }
            return "";
        }

        /// <summary>
        /// 从Request获取AppID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAppID(HttpRequestMessage request)
        {
            if (request != null)
            {
                return request.RequestUri.ParseQueryString()[WebConst.HttpParam_AppID];
            }
            return "";
        }

        public static double GetTimeSpan(string beginTime)
        {
            long lBeginTime;
            if (!long.TryParse(beginTime, out lBeginTime))
            {
                return -1;
            }
            TimeSpan begin = new TimeSpan(lBeginTime);
            TimeSpan end = new TimeSpan(DateTime.Now.Ticks);
            return (end - begin).TotalMilliseconds;
        }

        /// <summary>
        /// 根据参数检查加密串是否正确
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="md5Key"></param>
        /// <returns></returns>
        public static bool CheckUrlEncrypt(Uri uri, string queryStr, string md5Key, out string trueEncryptText)
        {
            trueEncryptText = "";
            List<string> list = new List<string>();

            if (queryStr.IndexOf("?") == 0)
            {
                queryStr = queryStr.Substring(1);
            }
            string[] querys = queryStr.Split('&');
            foreach (string q in querys)
            {
                if (!q.ToLower().StartsWith(WebConst.HttpParam_Encrypt.ToLower()))
                {
                    list.Add(q);
                }
            }
            list.Sort();

            string md5Query = uri.ParseQueryString()[WebConst.HttpParam_Encrypt];
            StringBuilder sBuilder = new StringBuilder();
            foreach (string q in list)
            {
                sBuilder.Append(q);
            }
            string toEncrypt = sBuilder.ToString() + md5Key;
            LogManager.DefaultLogger.Debug(string.Format("toEncrypt={0}", toEncrypt));
            string md5New = SecureHelper.MD5(toEncrypt);
            trueEncryptText = md5New;
            return md5New.CompareTo(md5Query) == 0;
        }

        public static string GetPostData(HttpRequestMessage request)
        {
            HttpContextBase context = null;

            if (request.Properties != null && request.Properties.Count > 0)
            {
                if (request.Properties["MS_HttpContext"] != null)
                {
                    context = (HttpContextBase)(request.Properties["MS_HttpContext"]);
                }                
            }            

            if (context == null || context.Request == null || context.Request.Form == null || context.Request.Form.Count == 0)
            {
                return string.Empty;
            }
            
            string ret = string.Empty;
            foreach(var key in context.Request.Form.Keys)
            {
                ret += string.Format("{0}={1}&", key.ToString(), context.Request.Form[key.ToString()]);
            }

            ret = ret.TrimEnd('&');

            return ret;
        }
    }
}
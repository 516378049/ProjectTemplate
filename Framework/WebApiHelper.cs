using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Framework
{
    public class WebApiHelper
    {
        /// <summary>
        /// 按照OpenAPI规则生成对应URL，主要为原始URL增加签名
        /// </summary>
        /// <param name="sUrl"></param>
        /// <param name="md5Key"></param>
        /// <returns></returns>
        public static string CreateUrl(string sUrl, string md5Key)
        {
            Uri uri;
            if (!Uri.TryCreate(sUrl, UriKind.RelativeOrAbsolute, out uri))
            {
                return sUrl;
            }

            List<string> list = new List<string>();
            string queryStr = uri.Query;
            if (queryStr.IndexOf("?") == 0)
            {
                queryStr = queryStr.Substring(1);
            }
            string[] querys = queryStr.Split('&');
            foreach (string q in querys)
            {
                list.Add(q);
            }
            list.Sort();
            list.Sort();
            StringBuilder sBuilder = new StringBuilder();
            foreach (string q in list)
            {
                sBuilder.Append(q);
            }
            string toEncrypt = sBuilder.ToString() + md5Key;
            string md5New = SecurityHelper.MD5(toEncrypt);
            return sUrl + "&encrypt=" + md5New;
        }



        /// <summary>
        /// 生成POST时加入数据签名的请求URL地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="md5key"></param>
        /// <returns></returns>
        public static string CreatePostUrl(string url, string data, string md5key, bool needUrlDecode = false)
        {

            Uri uri;
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                return url;
            }
            //具有公共参数不考虑无？情况。
            return url + "&encrypt=" + CreateEncryptKey(url + "&" + data, md5key, needUrlDecode);
        }

        /// <summary>
        /// 生成签名EncryptKey
        /// </summary>
        /// <param name="sUrl"></param>
        /// <param name="md5Key">encrypt</param>
        /// <param name="needUrlDecode">需要url解码</param>
        /// <returns></returns>
        public static string CreateEncryptKey(string sUrl, string md5Key, bool needUrlDecode = false)
        {
            //Uri uri;
            int index = sUrl.IndexOf("?");
            string queryStr = sUrl.Substring(index);
            //if (!Uri.TryCreate(sUrl, UriKind.RelativeOrAbsolute, out uri))
            //{
            //    queryStr = sUrl;
            //}
            //else
            //{
            //    queryStr = uri.Query;
            //}

            List<string> list = new List<string>();
            if (needUrlDecode)
            {
                //queryStr = HttpUtility.UrlDecode(uri.Query);
                queryStr = HttpUtility.UrlDecode(queryStr);
            }
            if (queryStr.IndexOf("?") == 0)
            {
                queryStr = queryStr.Substring(1);
            }
            string[] querys = queryStr.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string q in querys)
            {
                list.Add(q);
            }
            list.Sort();
            StringBuilder sBuilder = new StringBuilder();
            foreach (string q in list)
            {
                sBuilder.Append(q);
            }
            string toEncrypt = sBuilder.ToString() + md5Key;
            string md5New = SecurityHelper.MD5(toEncrypt);
            return md5New;
        }

        /// <summary>
        /// 获取POST请求加密串
        /// </summary>
        /// <param name="sUrl"></param>
        /// <param name="postData"></param>
        /// <param name="md5Key"></param>
        /// <returns></returns>
        public static string CreatePostEncryptKey(string sUrl, string postData, string md5Key)
        {
            Uri uri;
            if (!Uri.TryCreate(sUrl, UriKind.RelativeOrAbsolute, out uri))
            {
                return sUrl;
            }

            List<string> list = new List<string>();
            string queryStr = uri.Query;
            if (queryStr.IndexOf("?") == 0)
            {
                queryStr = queryStr.Substring(1);
            }
            string[] querys = queryStr.Split('&');
            foreach (string q in querys)
            {
                list.Add(q);
            }
            list.Sort();
            StringBuilder sBuilder = new StringBuilder();
            foreach (string q in list)
            {
                sBuilder.Append(q);
            }
            string toEncrypt = sBuilder.ToString() + postData + md5Key;
            string md5New = SecurityHelper.MD5(toEncrypt);
            LogManager.DefaultLogger.Info(toEncrypt + "==>" + md5New);
            return md5New;
        }

        class QueryInfo : IComparable
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public int CompareTo(object obj)
            {
                if (obj is QueryInfo)
                {
                    QueryInfo toCompare = obj as QueryInfo;
                    return this.Name.CompareTo(toCompare.Name);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// WebApi加密密钥
        /// </summary>
        private static string WebApiEncryptKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebApiEncryptKey"];
            }
        }

        /// <summary>
        /// 以Get协议调用WebApi
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string CallGetApi(string url, string query)
        {
            try
            {
                string apiUrl = CreateUrl(url + "?" + query, WebApiEncryptKey);
                HttpWebRequest request = WebRequest.Create(apiUrl) as HttpWebRequest;
                request.Method = "GET";
                request.KeepAlive = false;
                request.ContentType = "application/json";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                System.IO.Stream responseStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
                string srcString = reader.ReadToEnd();
                LogManager.WebApiLogger.Debug(string.Format("[apiUrl={0}] [ret={1}]", apiUrl, srcString));
                return srcString;
            }
            catch (WebException wex)
            {
                return wex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// POST发送数据
        /// 默认 UTF8
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data">name=2&age=23</param>
        /// <returns></returns>
        public static string PostData(string url, string data)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            byte[] bdata = Encoding.UTF8.GetBytes(data);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bdata.Length;
            request.KeepAlive = true;

            //使用cookies
            //requestScore.CookieContainer = ...;
            Stream stream = request.GetRequestStream();
            stream.Write(bdata, 0, bdata.Length);
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();

            request = null;
            response.Close();
            response = null;
            reader = null;
            stream = null;

            return content;
        }

        public static string GetParamsString(Dictionary<string, string> dicParms)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> item in dicParms)
            {
                sb.Append(string.Concat(item.Key, "=", item.Value, "&"));
            }
            string ret = sb.ToString();
            ret = ret.TrimEnd('&');
            return ret;
        }
    }
}

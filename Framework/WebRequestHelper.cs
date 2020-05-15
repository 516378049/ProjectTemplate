using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Framework
{
    public class WebRequestHelper
    {
        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private string encoding = "utf-8";
        private string contentType = "application/x-www-form-urlencoded";
        private X509Certificate2 certificate2 = null;

        #region 通讯函数 
        /// <summary>
        /// 通讯函数
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="para">请求参数</param>
        /// <param name="method">请求方式GET/POST</param>
        /// <returns></returns>
        public string SendRequest(string url, string para, string method)
        {
            string strResult = "";
            if (url == null || url == "")
                return null;
            if (method == null || method == "")
                method = "GET";
            // GET方式
            if (method.ToUpper() == "GET")
            {
                try
                {
                    HttpWebRequest wrq = (HttpWebRequest)WebRequest.Create(url + para);
                    wrq.Method = "GET";
                    foreach (string key in headers.Keys)
                    {
                        wrq.Headers.Add(key, headers[key]);
                    }

                    if (certificate2 != null)
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        ServicePointManager.Expect100Continue = true;
                        //添加验证证书的回调方法
                        ServicePointManager.ServerCertificateValidationCallback =
                        (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
                        wrq.ClientCertificates.Add(certificate2);
                    }
                    System.Net.WebResponse wrp = wrq.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(wrp.GetResponseStream(), System.Text.Encoding.GetEncoding(encoding));
                    strResult = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            // POST方式
            if (method.ToUpper() == "POST")
            {
                if (para.Length > 0 && para.IndexOf('?') == 0)
                {
                    para = para.Substring(1);
                }
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //WebRequest req = WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = contentType;

                foreach (string key in headers.Keys)
                {
                    req.Headers.Add(key, headers[key]);
                }

                if (certificate2 != null)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    ServicePointManager.Expect100Continue = true;
                    //添加验证证书的回调方法
                    ServicePointManager.ServerCertificateValidationCallback =
                    (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
                    req.ClientCertificates.Add(certificate2);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (para != null)
                {
                    int i = 0, j;
                    while (i < para.Length)
                    {
                        j = para.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            UrlEncoded.Append(HttpUtility.UrlEncode(para.Substring(i, para.Length - i), System.Text.Encoding.GetEncoding(encoding)));
                            break;
                        }
                        UrlEncoded.Append(HttpUtility.UrlEncode(para.Substring(i, j - i), System.Text.Encoding.GetEncoding(encoding)));
                        UrlEncoded.Append(para.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                try
                {
                    WebResponse result = req.GetResponse();
                    Stream ReceiveStream = result.GetResponseStream();
                    Byte[] read = new Byte[512];
                    int bytes = ReceiveStream.Read(read, 0, 512);
                    while (bytes > 0)
                    {
                        // 注意：
                        // 下面假定响应使用 UTF-8 作为编码方式。
                        // 如果内容以 ANSI 代码页形式（例如，932）发送，则使用类似下面的语句：
                        // Encoding encode = System.Text.Encoding.GetEncoding("shift-jis");
                        Encoding encode = System.Text.Encoding.GetEncoding(encoding);
                        strResult += encode.GetString(read, 0, bytes);
                        bytes = ReceiveStream.Read(read, 0, 512);
                    }
                    return strResult;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return strResult;
        }
        #endregion

        #region 简化通讯函数
        /// <summary>
        /// POST方式通讯
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Post(string url,string param)
        {
            return SendRequest(url, param, "POST");
        }

        /// <summary>
        /// GET方式通讯
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Get(string url,string param = "")
        {
            return SendRequest(url, param, "GET");
        }
        #endregion

        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetHeader(string name, string value)
        {
            if (headers.ContainsKey(name))
                headers[name] = value;
            else
                headers.Add(name, value);
        }

        /// <summary>
        /// 删除请求头
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void RemoveHeader(string name)
        {
            if (headers.ContainsKey(name))
                headers.Remove(name);
        }

        /// <summary>
        /// 获取请求头内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetHeader(string name)
        {
            if (headers.ContainsKey(name))
                return headers[name];
            return "";
        }

        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="encodingName"></param>
        public void SetEncoding(string encodingName)
        {
            encoding = encodingName;
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public string GetEncoding()
        {
            return encoding;
        }

        /// <summary>
        /// 添加证书
        /// </summary>
        /// <param name="cert"></param>
        public void AddCert(X509Certificate2 cert)
        {
            certificate2 = cert;
        }

        /// <summary>
        /// 设置ContentType
        /// </summary>
        /// <param name="type"></param>
        public void SetContentType(string type)
        {
            contentType = type;
        }
    }
}

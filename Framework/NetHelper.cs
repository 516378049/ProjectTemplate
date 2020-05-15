//using Runda.B2B.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ThinkDev.Logging;

namespace Framework
{
    public class NetHelper
    {
        #region 发送Email邮件
        /// <summary>
        /// 发送Email邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="bodyContent">内容</param>
        /// <param name="mailto">收件人邮箱地址</param>
        /// <param name="fromDisplayName">显示名</param>
        /// <param name="filename">附件文件地址</param>
        /// <param name="mailccto">抄送人邮箱地址</param>
        /// <returns></returns>
        public static int SendEmail(string subject, string bodyContent, string mailto, string fromDisplayName = "",
            byte[] buffer = null, string filename = "", string mailccto = "", string mailbccto = "")
        {
            if (string.IsNullOrEmpty(mailto))
            {
                return -9999;
            }
            using (MailMessage mailObj = new MailMessage())
            {
                try
                {
                    List<string> mailto_L = new List<string>();
                    mailto_L = mailto.Split(';').ToList();

                    List<string> mailccto_L = new List<string>();
                    mailccto_L = mailccto.Split(';').ToList();

                    List<string> mailbccto_L = new List<string>();
                    mailbccto_L = mailbccto.Split(';').ToList();

                    SmtpClient smtp = new SmtpClient(); //实例化一个 SmtpClient
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //将smtp的出站方式设为   Network
                    smtp.EnableSsl = false;//smtp服务器是否启用SSL加密
                    smtp.Host = ConfigHelper.Email_SMTP; //指定 smtp 服务器地址
                    smtp.Port = 25;    //指定 smtp 服务器的端口，默认是25，如果采用默认端口，可省去

                    smtp.UseDefaultCredentials = true;//如果需要认证，则用下面的方式
                    smtp.Credentials = new NetworkCredential(ConfigHelper.Email_UserName, ConfigHelper.Email_PWD);
                    MailMessage mm = new MailMessage(); //实例化一个邮件类

                    if (buffer != null)    //添加附件
                    {
                        MemoryStream memoryStream = new MemoryStream(buffer);

                        mm.Attachments.Add(new Attachment(memoryStream, filename));
                    }

                    //mm.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
                    mm.From = new MailAddress(ConfigHelper.Email_SendFrom, fromDisplayName, Encoding.GetEncoding(936));//收件方看到的邮件来源；
                    mm.Sender = new MailAddress(ConfigHelper.Email_SendFrom, fromDisplayName, Encoding.GetEncoding(936));

                    mailto = GetMailToWithoutExcludeMailTo(mailto); //获取排除的邮箱以外的发送邮箱

                    if (string.IsNullOrEmpty(mailto))
                    {
                        return 0;
                    }

                    foreach (var mail in mailto_L)
                    {
                        mm.To.Add(new MailAddress(mail.Trim()));//邮件的接收者，支持群发，多个地址之间用 半角逗号 分开//当然也可以用全地址添加 
                    }
                    //mm.To.Add(mailto);//邮件的接收者，支持群发，多个地址之间用 半角逗号 分开//当然也可以用全地址添加 

                    foreach (var mail in mailccto_L)    //抄送人
                    {
                        mm.CC.Add(new MailAddress(mail.Trim()));
                    }

                    foreach (var mail in mailbccto_L) //密抄送人
                    {
                        mm.Bcc.Add(new MailAddress(mail.Trim()));
                    }

                    mm.Subject = subject; //邮件标题
                    mm.SubjectEncoding = Encoding.GetEncoding(936);// 这里非常重要，如果你的邮件标题包含中文，这里一定要指定，否则对方收到的极有可能是乱码。
                    //936是简体中文的pagecode，如果是英文标题，这句可以忽略不用
                    mm.IsBodyHtml = true; //邮件正文是否是HTML格式
                    mm.BodyEncoding = Encoding.GetEncoding(936);//邮件正文的编码， 设置不正确， 接收者会收到乱码
                    mm.Body = bodyContent;//邮件正文

                    smtp.Send(mm);
                    return 0;
                }
                catch (Exception ex)
                {
                    LogManager.DefaultLogger.Error(LogConvert.ParseComplexEx(ex), string.Concat("SendEmail 发生异常。", subject, "。", mailto));
                    return -9999;
                }
            }
        }

        /// <summary>
        /// 获取排除的邮箱以外的发送邮箱
        /// </summary>
        /// <returns></returns>
        public static string GetMailToWithoutExcludeMailTo(string mailto)
        {
            string strExcludeMailTo = ConfigHelper.ExcludeMailTo;   //排除的邮箱

            if (string.IsNullOrEmpty(strExcludeMailTo))
            {
                return mailto;
            }

            List<string> excludeMailToList = new List<string>();
            string[] arrExcludeMailTo = strExcludeMailTo.Split(',');

            foreach (string mailTo in arrExcludeMailTo)
            {
                if (string.IsNullOrEmpty(mailTo))
                {
                    continue;
                }

                if (excludeMailToList.Contains(mailTo))
                {
                    continue;
                }

                excludeMailToList.Add(mailTo);
            }

            string mailtoAddress = string.Empty;
            string[] arrMailTo = mailto.Split(',');

            foreach (string mail in arrMailTo)
            {
                if (!excludeMailToList.Contains(mail))
                {
                    mailtoAddress += mail + ",";
                }
            }

            mailtoAddress = mailtoAddress.TrimEnd(",".ToCharArray());

            return mailtoAddress;
        }
        #endregion

        private string method = "GET";
        public System.Text.Encoding encode = Encoding.GetEncoding("utf-8");

        /// <summary>
        /// 默认获取客户端IP方式，当前系统默认为使用CDN方式
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIP()
        {
            return GetWebClientIP_CDN();
        }

        /// <summary>
        /// CDN方式获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIP_CDN()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return "127.0.0.1";
            }
            string IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            //CDN方式使用
            string ip_cdn = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip_cdn))
            {
                ip_cdn = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                string[] tmp = ip_cdn.Split(',');
                ip_cdn = tmp[0];
            }
            return ip_cdn;
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static string GetHostIP()
        {
            IPAddress[] hostList = Dns.GetHostAddresses(Dns.GetHostName());
            return hostList[0].ToString();
        }

        /// <summary>
        /// GET方式提交，可指定字符集
        /// </summary>
        /// <param name="UrlString"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string GetClientBySocket(string UrlString, System.Text.Encoding encode)
        {
            this.encode = encode;
            return GetClientBySocket(UrlString);
        }

        /// <summary>
        /// GET方式，默认字符集，本项目为gbk
        /// </summary>
        /// <param name="UrlString"></param>
        /// <returns></returns>
        public string GetClientBySocket(string UrlString, string reqMethod = "GET")
        {
            try
            {
                method = reqMethod;
                byte[] data = GetData(UrlString);
                return encode.GetString(data);
            }
            catch
            {
                return "";
            }
        }

        public byte[] GetData(string url)
        {
            return GetData(url, "");
        }

        private byte[] GetData(string UrlString, string updata)
        {
            string host;
            int port = 80;
            if (UrlString.StartsWith("http://"))
            {
                string temp = UrlString.Trim();
                temp = temp.Substring(7, temp.Length - 7);
                if (temp.IndexOf("/") == -1)
                {
                    host = temp;
                }
                else
                {
                    host = temp.Substring(0, temp.IndexOf("/"));
                }
                int portIndex = host.IndexOf(":");
                if (portIndex != -1)
                {
                    port = int.Parse(host.Substring(portIndex + 1));
                    host = host.Substring(0, portIndex);
                }
            }
            else
            {
                host = UrlString.Substring(0, UrlString.IndexOf("/"));
            }

            IPAddress[] ips = Dns.GetHostAddresses(host);
            IPAddress ip = ips[0];
            foreach (IPAddress _ip in ips)
            {
                if (_ip.AddressFamily.ToString() == "InterNetwork")
                    ip = _ip;
            }
            IPEndPoint serverhost = new IPEndPoint(ip, port);
            Socket clientSocket = null;
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(serverhost);

                string httpReq = method + " " + UrlString + " HTTP/1.1\r\n";
                httpReq += "Host:" + host + "\r\n";
                httpReq += "Content-Type: application/x-www-form-urlencoded;Charset=" + encode.BodyName + "\r\n";
                if (method.ToUpper().Trim() == "POST")
                {
                    httpReq += "Content-length:" + updata.Length + "\r\n";
                }
                httpReq += "Connection: close\r\n\r\n";
                httpReq += updata;
                StringBuilder txtHTML = new StringBuilder();

                clientSocket.Send(encode.GetBytes(httpReq));
                Byte[] buffer = new byte[10240];
                int byteCount = clientSocket.Receive(buffer, buffer.Length, 0);
                List<byte> bLst = new List<byte>();
                AddByte(bLst, buffer, byteCount);
                while (byteCount > 0)
                {
                    byteCount = clientSocket.Receive(buffer, buffer.Length, 0);
                    AddByte(bLst, buffer, byteCount);

                }
                byte[] bbs = new byte[bLst.Count];
                for (int i = 0; i < bLst.Count; i++)
                {
                    bbs[i] = bLst[i];
                }

                var str = encode.GetString(bbs, 0, bbs.Length);
                //判断页面状态
                string strState = string.Empty;
                for (int i = 0; i < bbs.Length; i++)
                {
                    if (bbs[i] == '\r' && bbs[i + 1] == '\n')
                    {
                        strState = this.encode.GetString(bbs, 0, i);
                        break;
                    }
                }
                var ss = strState.Substring(strState.IndexOf(' ') + 1);
                if (ss != "200 OK")
                {
                    throw new Exception(ss);
                }
                int dataIndex = 0;
                for (int i = 0; i < bbs.Length; i++)
                {
                    if (bbs[i] == '\r' && bbs[i + 1] == '\n' && bbs[i + 2] == '\r' && bbs[i + 3] == '\n')
                    {
                        dataIndex = i + 4;
                        break;
                    }
                }
                byte[] data = new byte[bbs.Length - dataIndex];
                for (int i = dataIndex, j = 0; i < bbs.Length; i++, j++)
                {
                    data[j] = bbs[i];
                }
                return data;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message + "url:" + UrlString);
                throw err;
            }
            finally
            {
                if (clientSocket != null)
                {
                    try
                    {
                        clientSocket.Close();
                    }
                    catch
                    {

                    }
                }
            }
        }

        void AddByte(List<byte> lst, byte[] data, int count)
        {
            for (int i = 0; i < count; i++)
            {
                lst.Add(data[i]);
            }
        }

        public static string PostData(string postcontent, string url)
        {
            byte[] byte1 = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(postcontent);

            string urlPage = url;
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;

            Encoding encoding = Encoding.GetEncoding("UTF-8");
            //byte[] data = encoding.GetBytes(xmlMsg);
            // 准备请求...
            // 设置参数
            // HttpWebRequest 自动注册。 不需要调用 RegisterPrefix 方法来注册 System.Net.HttpWebRequest 之前使用 Uri 开头 http:// 或 https://。
            request = WebRequest.Create(urlPage) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";

            //request.ContentType = "multipart/form-data";
            request.ContentLength = byte1.Length;
            outstream = request.GetRequestStream();
            outstream.Write(byte1, 0, byte1.Length);
            outstream.Flush();
            outstream.Close();
            //发送请求并获取相应回应数据
            response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            instream = response.GetResponseStream();
            sr = new StreamReader(instream, encoding);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            sr.Close();
            return content;
        }

        /// <summary>
        /// ContentType为application/x-www-form-urlencoded时所用
        /// </summary>
        /// <param name="postcontent"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string PostDataNoJoson(string postcontent, string url)
        {
            string ret = string.Empty;

            LogManager.DefaultLogger.Debug(string.Concat("转化前地址：", url, "转化前数据：", postcontent));
            byte[] byteArray = Encoding.UTF8.GetBytes(postcontent); //转化 /

            HttpWebRequest webReq = WebRequest.CreateHttp(url);
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";

            Stream newStream = webReq.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);//写入参数
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            ret = sr.ReadToEnd();
            LogManager.DefaultLogger.Debug(string.Concat("返回结果：", ret));
            sr.Close();
            response.Close();
            newStream.Close();

            return ret;
        }

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        /// <param name="encodingStr"></param>
        /// <param name="rename">为0时不重命名</param>
        /// <returns></returns>
        public static ApiRet UploadFile(string url, string path, string filename, byte[] data, string encodingStr = "", string rename = "")
        {
            string fileuploadUrl = "";
            if (string.IsNullOrEmpty(url))
            {
                //默认是上传图片的地址，不可以上传图片以外的文件
                fileuploadUrl = ConfigHelper.UploadUrl;
            }
            else
            {
                fileuploadUrl = url;
            }
            HttpUploadFile u = new HttpUploadFile();
            if (!string.IsNullOrEmpty(encodingStr))
                u.SetEncoding(Encoding.GetEncoding(encodingStr));
            string result = u.UploadFile(fileuploadUrl, path, filename, data, rename);
            ApiRet obj = new ApiRet();
            if (!string.IsNullOrEmpty(result))
                obj = JsonHelper.ToObject<ApiRet>(result);
            return obj;
        }

        public static ApiRet UploadFile(string path, string filename, byte[] data)
        {
            return UploadFile("", path, filename, data);
        }

        /// <summary>
        /// 根据URL读取文件，并返回二进制流的base64字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFileByURL(string url)
        {
            try
            {
                Stream stream = null;
                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;
                //以数组的形式下载指定文件  
                byte[] byteData = webClient.DownloadData(url);
                stream = BytesToStream(byteData);
                return System.Convert.ToBase64String(byteData);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>  
        /// 将二进制转化为数据流  
        /// </summary>  
        /// <param name="bytes">二进制数组</param>  
        /// <returns></returns>  
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        /// <summary>  
        /// 将流转化为二进制数组  
        /// </summary>  
        /// <param name="stream"></param>  
        /// <returns></returns>  
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始     
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}

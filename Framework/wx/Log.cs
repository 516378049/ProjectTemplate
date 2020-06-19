using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;

namespace WxPayAPI
{
    public class Log
    {
        static string GlobalPath = "C:\\weblogs\\wxJsApiPay\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day+"\\";
        static string GlobalFile = "globalLogs.txt";

        /**
         * 向日志写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            if(WxPayConfig.GetConfig().GetLogLevel() >= 3)
            {
                WriteLog("DEBUG", className, content, GlobalPath, GlobalFile);
            }
        }

        /**
        * 向日志写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            if(WxPayConfig.GetConfig().GetLogLevel() >= 2)
            {
                WriteLog("INFO", className, content, GlobalPath, GlobalFile);
            }
        }

        /**
        * 向日志写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
            if(WxPayConfig.GetConfig().GetLogLevel() >= 1)
            {
                
                WriteLog("ERROR", className, content, GlobalPath, GlobalFile);
            }
        }

        /**
        * 向日志写入用户访问信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void UserVisitInfo(string className, string content)
        {
            content = HttpContext.Current.Request.UserHostName + "[" + HttpContext.Current.Request.UserHostAddress + "] " + content;
            if (WxPayConfig.GetConfig().GetLogLevel() >= 1)
            {
                string Path = "C:\\weblogs\\wxJsApiPay\\";
                string File = "UserVisitInfo.txt";
                WriteLog("UserVisitInfo", className, content, Path, File);
            }
        }


        /**
        * 向日志写入用户授权信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void UserAuthorizeInfo(string className, string content)
        {
            content = HttpContext.Current.Request.UserHostName + "[" + HttpContext.Current.Request.UserHostAddress + "] "+ content;
            if (WxPayConfig.GetConfig().GetLogLevel() >= 1)
            {
                string Path = "C:\\weblogs\\wxJsApiPay\\";
                string File = "UserAuthorizeInfo.txt";
                WriteLog("UserAuthorizeInfo", className, content, Path, File);
            }
        }

        /**
        * 向日志写入用户支付信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void UserPayInfo(string className, string content)
        {
            content = HttpContext.Current.Request.UserHostName + "[" + HttpContext.Current.Request.UserHostAddress + "] " + content;
            if (WxPayConfig.GetConfig().GetLogLevel() >= 1)
            {
                string Path = "C:\\weblogs\\wxJsApiPay\\";
                string File = "UserPayInfo.txt";
                WriteLog("UserPayInfo", className, content, Path, File);
            }
        }

        /**
        * 实际的写日志操作
        * @param type 日志记录类型
        * @param className 类名
        * @param content 写入内容
        */
        protected static void WriteLog(string type, string className, string content,string path,string file)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            //日志内容
            string write_content = time + " " + type + " " + className + ": " +content;
            //需要用户自定义日志实现形式
            Console.WriteLine(write_content);
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            StringBuilder log_content = new StringBuilder("");
            log_content.AppendLine(write_content);
            File.AppendAllText(path+ file, log_content.ToString());
        }
    }
}
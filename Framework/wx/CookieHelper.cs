using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WxPayAPI
{
    public class CookieHelper
    {
        private static string Domain = "www.changchunamy.com";
        public static void SetCookie(string cookieName, string value)
        {
            //add by zhouyw for 在ifram中cookie不会丢失问题
            HttpContext.Current.Response.AddHeader("P3P", "CP=CAO PSA OUR");
            HttpCookie cookietwo = new HttpCookie(cookieName); 
            cookietwo.Value = value;
            cookietwo.Domain = Domain;
            HttpContext.Current.Response.Cookies.Add(cookietwo);
        }
        public static void SetCookie(string cookieName, string key, string value)
        {
            HttpCookie cookietwo = new HttpCookie(cookieName);
            cookietwo.Values[key] = value;


            cookietwo.Domain = Domain;

            HttpContext.Current.Response.Cookies.Add(cookietwo);
        }

        public static void SetCookie(string cookieName, string key, string value, int hours)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values[key] = value;


            cookie.Domain = Domain;


            cookie.Expires = DateTime.Now.AddHours(hours);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void SetCookie(string cookieName, int hours)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
                cookie.Domain = Domain;
                cookie.Expires = DateTime.Now.AddHours(hours);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static string GetCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                return HttpContext.Current.Request.Cookies[cookieName].Value;
            }
            return "";
        }


        public static string GetCookie(string cookieName, string key)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                if (HttpContext.Current.Request.Cookies[cookieName][key] != null)
                {
                    return HttpContext.Current.Request.Cookies[cookieName][key].ToString();
                }
            }
            return "";
        }

        public static void RemoveCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}

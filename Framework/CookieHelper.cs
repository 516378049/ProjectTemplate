using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework
{
    public class CookieHelper
    {
        public static void SetCookie(string cookieName, string value)
        {
            //add by zhouyw for 在ifram中cookie不会丢失问题
            HttpContext.Current.Response.AddHeader("P3P", "CP=CAO PSA OUR");
            HttpCookie cookietwo = new HttpCookie(cookieName);
            cookietwo.Value = value;

            if (ConfigHelper.MainDomain.Contains("mai47.com"))
            {
                cookietwo.Domain = "mai47.com";
            }
            
            HttpContext.Current.Response.Cookies.Add(cookietwo);
        }

        public static void SetCookie(string cookieName, string key, string value)
        {
            HttpCookie cookietwo = new HttpCookie(cookieName);
            cookietwo.Values[key] = value;
            
            if (ConfigHelper.MainDomain.Contains("mai47.com"))
            {
                cookietwo.Domain = "mai47.com";
            }

            //cookietwo.Expires = DateTime.Now.AddHours(24);
            HttpContext.Current.Response.Cookies.Add(cookietwo);
        }

        public static void SetCookie(string cookieName, string key, string value, int hours)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values[key] = value;
            
            if (ConfigHelper.MainDomain.Contains("mai47.com"))
            {
                cookie.Domain = "mai47.com";
            }

            cookie.Expires = DateTime.Now.AddHours(hours);
            HttpContext.Current.Response.Cookies.Add(cookie);
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

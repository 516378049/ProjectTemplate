using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using Newtonsoft.Json;
using Model.Common;

namespace Framework
{
    public class UserOptionLog:ActionFilterAttribute
    {

        public static string requestParams { get; set; }
        public static string responseParams { get; set; }//get value in the page of baseController
        public static string user { get; set; }

        //
        // 摘要:
        //     在执行操作结果后由 ASP.NET MVC 框架调用。
        //
        // 参数:
        //   filterContext:
        //     筛选器上下文。
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            List<HashKeyValue> requestStr = new List<HashKeyValue>();

            if(filterContext.HttpContext.Request.RequestType=="GET")
            {
                for (int i = 0; i < filterContext.HttpContext.Request.QueryString.Count; i++)
                {
                    string key = filterContext.HttpContext.Request.QueryString.Keys[i];
                    string value = filterContext.HttpContext.Request.QueryString[key];
                    requestStr.Add(new HashKeyValue { key = key, value = value });
                }
            }
            else
            {
                for (int i = 0; i < filterContext.HttpContext.Request.Form.Count; i++)
                {
                    string key = filterContext.HttpContext.Request.Form.Keys[i];
                    string value = filterContext.HttpContext.Request.Form[key];
                    requestStr.Add(new HashKeyValue { key = key, value = value });
                }
            }
            
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            requestParams=JsonConvert.SerializeObject(requestStr, setting);
            
            //write the code to save to dataBase here 

            string Controller = filterContext.RouteData.Values["action"].ToString()+ "/"+ filterContext.RouteData.Values["controller"].ToString();
            base.OnResultExecuted(filterContext);
        }
      
    }
}
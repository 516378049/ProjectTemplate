using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAPI.Core.WebAPI;

namespace Vue
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "MainApi_Dot",
                routeTemplate: "api/{controller}.{action}",
                defaults: new { id = RouteParameter.Optional }
            );
            //config.Routes.MapHttpRoute(
            //    name: "MainApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //这段代码如果启用，则会屏蔽返回类型XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //注册自定义消息处理器
            config.MessageHandlers.Add(new ApiValidateHandler());
        }
    }
}
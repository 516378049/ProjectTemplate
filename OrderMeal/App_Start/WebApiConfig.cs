using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAPI.Core.WebAPI;

namespace OrderMeal
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //默认路由
            //自定义路由
            //路由特性 1、简单的路由特性 2、带参数{id}占位符的路由，参数约束和默认值 3、路由前缀
            //路由文档 https://www.cnblogs.com/landeanfen/p/5501490.html
            // Web API 配置和服务
            //如果路由模板配置了{action}，那么找到对应的action就很简单，如果没有配置action，则会首先匹配请求类型（get/post/put/delete等），然后匹配请求参数，找到对应的action
            // Web API 路由
            //config.MapHttpAttributeRoutes();//启用特性路由

            //1、默认路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //2、自定义路由一：匹配到action，如果action有多个参数，则;actionapi.values.seller/?a1=1&b1=2
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api.{controller}.{action}/{id}",//1、{id}设置或不设置都可以，访问地址必须要以‘/’结尾 2、{id}前必须为'/',不能为'.'，最后如果有参数则以‘/{id}’结尾，没有参数的话以'/'结尾
                defaults: new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(new ApiValidateHandler());
        }
    }
}

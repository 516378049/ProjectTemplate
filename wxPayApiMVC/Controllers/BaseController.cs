using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WxPayAPI;
using wxPayApiMVC.lib;
namespace wxPayApiMVC.Controllers
{
    public class BaseController : Controller
    {
        #region 统一异常处理
        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Error(this.GetType().ToString(), "error : " + filterContext.Exception.Message + "<br>" + filterContext.Exception.StackTrace);
            Log.Debug("userAgent", filterContext.HttpContext.Request.UserAgent);
           // 标记异常已处理
            filterContext.ExceptionHandled = true;

            if (filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.HttpContext.Request.UserAgent.Contains("miniProgram"))   //判断是否ajax请求
            {
                filterContext.Result = Json(new{ retMsg = filterContext.Exception.Message});
            }
            else
            {
                // 跳转到错误页
                filterContext.Result = new RedirectResult(Url.Action("Error", "Common", new { error = filterContext.Exception.Message }));
            }
        }
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        #region common function
        //public void logTest(string content) {
        //    //string cache = HttpContext.Cache.Get("orderId")==null?"null":HttpContext.Cache.Get("orderId").ToString();
        //    //content = "cache:"+cache + " state:" +(Session["state"]==null?"null": Session["state"].ToString())+ (SessionHelper.GetSession("testSeeion")==null?"null": SessionHelper.GetSession("testSeeion").ToString())+" "+content+ "timeOut:"+Session.Timeout.ToString()+ " ,fee:" + (Session["totalFee"] == null ? "null" : Session["totalFee"].ToString())+" ,orderId:" + (Session["orderId"] == null ? "null" : Session["orderId"].ToString())+",testSeesion:"+ (HttpContext.Session["totalFee22"]==null?"null": HttpContext.Session["totalFee22"].ToString()) ;
        //    Log.Debug("logTest", content);
        //}

        /// <summary>
        /// 转化JsonResult
        /// </summary>
        /// <param name="JR"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string ConvertJsonResult(JsonResult JR, string Key)
        {
            object obj = JR.Data;
            JsonSerializerSettings setting = new JsonSerializerSettings();
            // 设置日期序列化的格式
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            string JsonStrValue = JsonConvert.SerializeObject(obj, setting);
            dynamic modelDy = JsonConvert.DeserializeObject<dynamic>(JsonStrValue);
            object ret = modelDy[Key];
            return ret.ToString();
        }
        /// <summary>
        /// 转化JsonResult
        /// </summary>
        /// <param name="JR"></param>
        /// <returns></returns>
        public string ConvertJsonResult(JsonResult JR)
        {
            object obj = JR.Data;
            JsonSerializerSettings setting = new JsonSerializerSettings();
            // 设置日期序列化的格式
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            string JsonStrValue = JsonConvert.SerializeObject(obj, setting);
            return JsonStrValue;
        }
        #endregion

    }
}
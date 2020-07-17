using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.EF;
using Runda.B2B.Framework;

namespace Vue
{
    public class BaseController : Controller
    {
        public UnitOfWork Studio = new UnitOfWork(true);
        public T GetObj<T>() {
            return SingleObj<T>.getObj;
        }
        /// <summary>
        /// 创建ApiResult对象，所有WebApi接口均使用该方法包装返回值
        /// </summary>
        /// <param name="retCode"></param>
        /// <param name="retMsg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        //protected ActionResult CreateApiResult(object message = null, string retCode = "0", string retMsg = "", int Total = 0)
        //{
        //    return Json(new ApiResult() { RetCode = retCode, Total = Total, RetMsg = retMsg, Message = message }, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// 创建ApiResult对象，所有WebApi接口均使用该方法包装返回值
        /// </summary>
        /// <param name="retCode"></param>
        /// <param name="retMsg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected JsonResult CreateApiResult(object message = null, string retCode = "0", string retMsg = "", int Total = 0)
        {
            return Json(new ApiResult() { RetCode = retCode, Total = Total, RetMsg = retMsg, Message = message }, JsonRequestBehavior.AllowGet);
        }
        public class ApiResult
        {
            public string RetCode { get; set; }
            public string RetMsg { get; set; }
            public object Message { get; set; }
            public int Total { get; set; }
            public string ToJson()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}
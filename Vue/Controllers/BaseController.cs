using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Core.WebAPI;
using Vue.Models;
using Runda.B2B.Framework;

namespace Vue
{
    public class BaseController : Controller
    {
        public UnitOfWork Studio = new UnitOfWork();
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
        protected ActionResult CreateApiResult(object message = null, string retCode = "0", string retMsg = "", int Total = 0)
        {
            return Json(new ApiResult() { RetCode = retCode, Total = Total, RetMsg = retMsg, Message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}
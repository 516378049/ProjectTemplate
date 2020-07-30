using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using WebAPI.Core.WebAPI;
using Framework;

namespace WebAPI.Core.WebAPI
{
    public class ApiExceptionFilter : ExceptionFilterAttribute 
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //ErrorLog
            //LogManager.WebApiLogger.Error(LogConvert.ParseWebEx(context.Exception), "接口发生系统未捕捉异常");
            Log.ILog4_Error.Error("接口发生系统未捕捉异常",context.Exception);
            ApiResult result = new ApiResult() { RetCode = WebConst.RetCode_SysError, RetMsg = context.Exception.Message, Message = null };
            context.Exception = null;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(result.ToJson());
            context.Response = response;
        } 
    }
}

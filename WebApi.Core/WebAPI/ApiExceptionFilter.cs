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
            Log.ILog4_Error.Error("网络异常:接口统一异常处理", context.Exception);
            ApiResult result = new ApiResult() { RetCode = WebConst.RetCode_SysError, RetMsg = context.Exception.Message, Message = null };
            context.Exception = null;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(result.ToJson());
            context.Response = response;
        } 
    }
}

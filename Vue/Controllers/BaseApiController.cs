using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using CoreWebApi = WebAPI.Core.WebAPI;
using System;
using WebAPI.Core.WebAPI;
using System.ServiceModel.Channels;
using Framework;

namespace Vue.Controllers
{
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// 创建ApiResult对象，所有WebApi接口均使用该方法包装返回值
        /// </summary>
        /// <param name="retCode"></param>
        /// <param name="retMsg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected string CreateApiResult(string retCode, string retMsg, object message)
        {
            return JsonConvert.SerializeObject(new CoreWebApi.ApiResult() { RetCode = retCode, RetMsg = retMsg, Message = message });
        }

        protected string CreateApiResult(CoreWebApi.ApiResult result)
        {
            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// 创建ApiResult对象，所有WebApi接口均使用该方法包装返回值
        /// </summary>
        /// <param name="retCode"></param>
        /// <param name="retMsg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected ApiResult CreateApiResult(object message = null,string retCode = "0", int Total=0, string retMsg = "")
        {
            return new ApiResult() { RetCode = retCode,Total= Total, RetMsg = retMsg, Message = message };
        }
        /// <summary>
        /// 检查ApiResult是否表示正确结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool CheckResult(ApiResult result)
        {
            return result.RetCode == "0";
        }

        /// <summary>
        /// 从Request内获取ApiName
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string GetApiName(HttpRequestMessage request)
        {
            return CoreWebApi.WebFuncHelper.GetApiName(request);
        }

        #region 基础属性
        /// <summary>
        /// 获取当前请求的APPID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string AppID
        {
            get
            {
                return CoreWebApi.WebFuncHelper.GetHeadValue(Request, CoreWebApi.WebConst.Header_AppID);
            }
        }

        /// <summary>
        /// 获取当前请求的ApiName
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string ApiName
        {
            get
            {
                return CoreWebApi.WebFuncHelper.GetApiName(Request);
            }
        }

        #endregion

        /// <summary>
        /// 返回带有WebApi层标示的错误码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected string GetErrCode(string code)
        {
            return WebFuncHelper.GetErrCode(code);
        }

        /// <summary>
        /// 获得调用者IP
        /// </summary>
        /// <returns></returns>
        protected string GetClientIP()
        {
            if (Request.Properties.ContainsKey("MS_HttpContext"))
                return ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;


            else if (Request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)Request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取Post集合中的数据
        /// </summary>
        /// <param name="postKey"></param>
        /// <returns></returns>
        protected string GetRequestData(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            HttpContextBase context = (HttpContextBase)(Request.Properties["MS_HttpContext"]);

            if (context == null)
            {
                return "";
            }
            return context.Request[key];
        }


        /// <summary>
        /// 获取POST数据流中的数据键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetRequestStreamData(string key)
        {
            string POSTDATAKEY = WebConst.HttpPostData_Key;
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }

            if (HttpContext.Current.Items[POSTDATAKEY] == null)
            {
                HttpContextBase context = (HttpContextBase)(Request.Properties["MS_HttpContext"]);
                int len = (context.Request).ContentLength;
                byte[] bytes = new byte[len];
                (((System.Web.HttpContextWrapper)context).Request).InputStream.Read(bytes, 0, len);
                System.Text.UTF8Encoding converter = new System.Text.UTF8Encoding();
                string inputString = converter.GetString(bytes);
                System.Collections.Specialized.NameValueCollection nc = HttpUtility.ParseQueryString(inputString);

                HttpContext.Current.Items.Add(POSTDATAKEY, nc);
                if (nc == null)
                {
                    return "";
                }
                return nc[key];
            }
            else
            {
                System.Collections.Specialized.NameValueCollection nc = (System.Collections.Specialized.NameValueCollection)HttpContext.Current.Items[POSTDATAKEY];
                return nc[key];
            }

        }


        /// <summary>
        /// 使WebApi返回字符串
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected HttpResponseMessage ResponseString(string content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(content);
            return response;
        }

        /// <summary>
        /// 异常处理函数，并返回包含异常信息的ApiResult
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected ApiResult ErrorHandle(Exception ex, string actionInfo = "")
        {
            //TODO:加入异常统计
            Log.ILog4.Debug(Log.logExpConvert(ex));
            return new ApiResult()
            {
                RetCode = GetErrCode(WebConst.RetCode_Error),
                //RetMsg = actionInfo + "接口出现错误",
                RetMsg = string.Format("{0}接口出现错误 ({1})", actionInfo, ex.Message.ToString()),
                Message = null
            };
        }
    }
}

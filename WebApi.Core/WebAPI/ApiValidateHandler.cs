using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using System.Net;
using WebAPI.Core;
using System.ServiceModel.Channels;

using ThinkDev.Logging;
using WebAPI.Core.Models;
using WebAPI.Core.Business.App;
using ServiceStack.Common.Web;
using System.Collections.Specialized;
using DataAccess.EF;
using Model.EF;

namespace WebAPI.Core.WebAPI
{
    public class ApiValidateHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            #region 验证token
            UnitOfWork Studio = new UnitOfWork();
            var accesstoken = WebFuncHelper.GetHeadValue(request, "accesstoken");
            var UserId = int.Parse(WebFuncHelper.GetHeadValue(request, WebConst.Header_UserId));
            UserInfo userinfo = Studio.UserInfo.Get(X => X.Id==UserId && X.access_token ==accesstoken  && X.DelFlag == 0).FirstOrDefault();
            if (userinfo == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(CreateApiResult("","-1",0, "token验证失败").ToJson());
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            #endregion
            return base.SendAsync(request, cancellationToken)
            .ContinueWith(
                (task) =>
                {
                    var content = task.Result.Content as System.Net.Http.ObjectContent;
                    var apiResult = content.Value as ApiResult;

                    if (apiResult != null)
                    {
                        if (content.ObjectType == typeof(System.Web.Http.HttpError))
                        {
                            task.Result.StatusCode = HttpStatusCode.OK;
                            task.Result.Content = new StringContent(apiResult.ToJson());
                        }
                    }
                    else
                    {
                        throw new ApplicationException("未捕获异常,返回值不符合规则");
                    }
                    return task.Result;
                }
            );
        }

        /// <summary>
        /// 统一验证函数，需要注意的是验证错误返回Code为800001~899999
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private ApiResult Validate(HttpRequestMessage message)
        {
            ApiResult result = new ApiResult() { RetCode = "0", RetMsg = "" };
            var apiName = WebFuncHelper.GetApiName(message);
            var apiModule = WebFuncHelper.GetApiModule(message);
            int iAppId = 0;
            var appId = WebFuncHelper.GetAppID(message);
            var apiId = "0";
            AppInfo appInfo = null;
            ApiInfo apiInfo = null;
            App_ApiRelation relation = null;
            string trueEncryptText = "";
            string postData = "";
            try
            {

                #region 基本参数验证

                if (string.IsNullOrEmpty(appId))
                {
                    result.RetCode = WebFuncHelper.GetErrCode("800001");
                    result.RetMsg = "参数不完整";
                }

                if (result.RetCode == "0")
                {
                    if (!int.TryParse(appId, out iAppId))
                    {
                        result.RetCode = WebFuncHelper.GetErrCode("800002");
                        result.RetMsg = "参数格式不正确";
                    }
                }

                if (result.RetCode == "0")
                {
                    using (AppRule rule = new AppRule())
                    {
                        appInfo = rule.GetSingleAppInfo(iAppId);
                        if (appInfo == null)
                        {
                            result.RetCode = WebFuncHelper.GetErrCode("800003");
                            result.RetMsg = "指定的Appid不存在";
                        }
                        if (result.RetCode == "0")
                        {
                            apiInfo = rule.GetSingleApiInfo(apiModule, apiName);
                            if (apiInfo == null)
                            {
                                result.RetCode = WebFuncHelper.GetErrCode("800004");
                                result.RetMsg = "指定的Api未在平台登记";
                            }
                            else
                            {
                                apiId = apiInfo.ApiID.ToString();
                                if (!apiInfo.IsUse)
                                {
                                    result.RetCode = WebFuncHelper.GetErrCode("800005");
                                    result.RetMsg = "指定的Api目前尚未开放";
                                }
                            }
                        }
                        if (result.RetCode == "0")
                        {
                            relation = rule.GetRelation(iAppId, apiInfo.ApiID);
                            if (relation == null)
                            {
                                result.RetCode = WebFuncHelper.GetErrCode("800006");
                                result.RetMsg = "当前应用尚未开通指定的Api访问权限";
                            }
                            else
                            {
                                if (!relation.IsUse)
                                {
                                    result.RetCode = WebFuncHelper.GetErrCode("800007");
                                    result.RetMsg = "指定的Api拒绝当前应用访问[" + relation.DownInfo + "]";
                                }
                            }
                        }
                    }
                }
                #endregion

                #region IP验证逻辑
                if (result.RetCode == "0")
                {
                    if (!string.IsNullOrEmpty(appInfo.AppIPList))
                    {
                        string clientIP = "," + GetClientIP(message) + ",";
                        if (!appInfo.AppIPList.Contains(clientIP))
                        {
                            result.RetCode = WebFuncHelper.GetErrCode("800009");
                            result.RetMsg = "IP被拒绝" + clientIP;
                        }
                    }
                }
                #endregion

                #region 根据指定的验证方式验证
                if (result.RetCode == "0")
                {
                    if (apiInfo.ValidateType == 1)
                    {
                        var encrypt = message.RequestUri.ParseQueryString()[WebConst.HttpParam_Encrypt];
                        if (string.IsNullOrEmpty(appId))
                        {
                            result.RetCode = WebFuncHelper.GetErrCode("800001");
                            result.RetMsg = "参数不完整";
                        }
                        //Tip:MD5验证
                        string md5Key = appInfo.AppKey;
                        if (message.Method == HttpMethod.Post)
                        {
                            NameValueCollection nc = new NameValueCollection();

                            #region 解析Post数据流
                            HttpContextBase context = (HttpContextBase)(message.Properties["MS_HttpContext"]);
                            int len = (context.Request).ContentLength;
                            if (len > 0)
                            {
                                byte[] bytes = new byte[len];
                                (((System.Web.HttpContextWrapper)context).Request).InputStream.Read(bytes, 0, len);
                                System.Text.UTF8Encoding converter = new System.Text.UTF8Encoding();
                                postData = converter.GetString(bytes);
                                nc = HttpUtility.ParseQueryString(postData);
                                HttpContext.Current.Items.Add(WebConst.HttpPostData_Key, nc);
                                HttpContext.Current.Items.Add(WebConst.HttpPostString_Key, postData);
                            }
                            #endregion

                            if (nc.Count > 0)
                            {
                                if (!WebFuncHelper.CheckUrlEncrypt(message.RequestUri, message.RequestUri.Query + "&" + postData, md5Key, out trueEncryptText))
                                {
                                    result.RetCode = WebFuncHelper.GetErrCode("800010");
                                    result.RetMsg = "加密串验证失败";
                                }
                            }
                        }
                        else
                        {
                            if (!WebFuncHelper.CheckUrlEncrypt(message.RequestUri, message.RequestUri.Query, md5Key, out trueEncryptText))
                            {
                                result.RetCode = WebFuncHelper.GetErrCode("800010");
                                result.RetMsg = "加密串验证失败";
                            }
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                result.RetCode = WebFuncHelper.GetErrCode("899999");
                result.RetMsg = "接口验证发生异常";
                result.Message = null;
                //ErrorLog
                LogManager.WebApiLogger.Error(LogConvert.ParseWebEx(ex), "接口验证发生异常");
            }

            if (result.RetCode != "0")
            {
                //Log
                string logFormat = "[{0}] [{1}] PostData:[{2}] Ret:[{3}] TimeSpan:[{4}] Encrypt:[{5}]";
                double runTime = 0;
                if (message.Headers.Contains(WebConst.Header_BeginTime))
                {
                    runTime = WebFuncHelper.GetTimeSpan(message.Headers.GetValues(WebConst.Header_BeginTime).FirstOrDefault());
                }
                //如果请求的App与Api为非正常，则只记录文本日志，不记录数据库统计日志
                if (appInfo != null && apiInfo != null)
                {
                    AppRule.ApiAddCount(appId, apiId, apiModule, apiName, result.RetCode, 1);
                }
                else
                {
                    LogManager.WebApiLogger.WarnFormat(logFormat, apiName, message.RequestUri.ToString(), postData, result.ToJson(), runTime.ToString(), trueEncryptText);
                }

                //xuqing 2018.9.28 注释
                //LogManager.WebApiLogger.InfoFormat(logFormat, apiName, message.RequestUri.ToString(), postData, result.ToJson(), runTime.ToString(), trueEncryptText);
            }
            else
            {
                //在Head加入一些属性，以便后续使用
                message.Headers.Add(WebConst.Header_AppID, appId);
                message.Headers.Add(WebConst.Header_ApiID, apiId);
                message.Headers.Add(WebConst.Header_ApiName, apiName);
                message.Headers.Add(WebConst.Header_ApiModule, apiModule);
                message.Headers.Add(WebConst.Header_PostString, postData);
            }

            return result;
        }

        protected static string GetClientIP(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;


            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 记录API运行日志
        /// </summary>
        /// <param name="request"></param>
        /// <param name="result"></param>
        protected static void WriteApiLog(HttpRequestMessage request, ApiResult result)
        {
            bool isAutoWriteLog = !(WebFuncHelper.GetHeadValue(request, WebConst.Header_AutoWriteLogFlag) == "false");
            if (isAutoWriteLog)
            {
                try
                {
                    string logFormat = "[{0}] [{1}] PostData[{2}] Ret:[{3}] TimeSpan:[{4}]";
                    double runTime = WebFuncHelper.GetTimeSpan(WebFuncHelper.GetHeadValue(request, WebConst.Header_BeginTime));
                    var apiName = WebFuncHelper.GetHeadValue(request, WebConst.Header_ApiName);
                    string urlData = request.RequestUri.ToString();
                    //string postData = WebFuncHelper.GetHeadValue(request, WebConst.Header_PostString);
                    //if (request != null)  不记录WebApi_Info日志  xuqing 2018.9.28
                    //{
                    //    string postData = WebFuncHelper.GetPostData(request);
                    //    LogManager.WebApiLogger.Info(null, logFormat, apiName, urlData, postData, result.ToJson(), runTime.ToString());
                    //}

                    long monitorTime = Convert.ToInt64(runTime);
                    if (monitorTime >= ConfigHelper.GetMonitorLimitPageTime())
                    {
                        //PageMonitor.Send(request.RequestUri.ToString(), monitorTime, GetClientIP(request));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.DefaultLogger.Error(ex, "记录API运行日志失败");
                }
            }
        }

        /// <summary>
        /// 从HttpContent获取ApiResult
        /// </summary>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        protected static ApiResult GetApiResult(ObjectContent content, string apiName, string appId, string apiId, string apiModule)
        {
            var apiResult = content.Value as ApiResult;

            if (apiResult == null)
            {
                System.Web.Http.HttpError error = content.Value as System.Web.Http.HttpError;
                if (error != null)
                {
                    //如果只返回“发生错误”异常，请检查HttpPost/HttpGet 特性，是否引用于System.Web.Http
                    LogManager.WebApiLogger.Error(string.Format("GetApiResult 内发生异常！[message={0},apiName={1},appId={2},apiId={3},apiModule={4}]",
                        String.Join(" ", error.Values), apiName, appId, apiId, apiModule));

                    apiResult = new ApiResult() { RetCode = WebConst.RetCode_SysError, RetMsg = String.Join(" ", error.Values), Message = null };
                }
            }
            return apiResult;
        }
        /// <summary>
        /// 创建ApiResult对象，所有WebApi接口均使用该方法包装返回值
        /// </summary>
        /// <param name="retCode"></param>
        /// <param name="retMsg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected ApiResult CreateApiResult(object message = null, string retCode = "0", int Total = 0, string retMsg = "")
        {
            return new ApiResult() { RetCode = retCode, Total = Total, RetMsg = retMsg, Message = message };
        }
        
    }
    public class KValue
    {
        public string key { set; get; }
        public string value { set; get; }
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using System.Collections.Generic;
using WxPayAPI;
using System.Text;
using WxPayAPI.wxRsult;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace wxPayApiMVC
{
    /// <summary>
    /// 判断订单和费用是否合法
    /// </summary>
    public class VaildateApiParamas : ActionFilterAttribute
    {
        string openId = "";
        //string orderId = "";
        //string totalFee = "";
        private SortedDictionary<string, string> m_params = new SortedDictionary<string, string>();
        public VaildateApiParamas() { }
        public VaildateApiParamas(string Params)
        {
            List<string> _params= Params.Split('|').ToList();

            if (_params.Contains("orderId"))
            { this.openId = "true"; }
            //if (_params.Contains("totalFee"))
            //{ this.orderId = "true"; }
            //if (_params.Contains("openId"))
            //{ this.totalFee = "true"; }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获得参数
            m_params=getActionParams(filterContext);
            Log.Debug("userAgent", filterContext.HttpContext.Request.UserAgent);
            string err = ValidateOrderId_TotalFee(filterContext);
            if ( !string.IsNullOrEmpty(err))
            {

                if (filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.HttpContext.Request.UserAgent.Contains("miniProgram"))   //判断是否ajax请求或者小程序调用接口
                {
                    JsonResult JR = new JsonResult();
                    JR.Data = new { retMsg = err };

                    JR.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    filterContext.Result = JR;
                    //filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Common", action = "Error", error = err }));
                }
                else
                {
                    // 跳转到错误页
                    //filterContext.Result = new RedirectResult(Url.Action("Error", "Common", new { error = filterContext.Exception.Message }));
                    filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Common", action = "Error", error = err }));
                }
            }
        }
        /// <summary>
        /// validate orderId and fee
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="totalFee"></param>
        /// <returns></returns>
        private string ValidateOrderId_TotalFee(ActionExecutingContext filterContext)
        {
            //获取参数
            string orderId = string.Empty;
            string totalFee = string.Empty;

            orderId = getActionParams(m_params,"orderId");
            totalFee = getActionParams(m_params, "totalFee");

            #region 判断单号和金额是否正常

            StringBuilder error = new StringBuilder("");

            int _totalFee = 0;

            //判断openId 参数
            if (!string.IsNullOrEmpty(openId))
            {
                if(string.IsNullOrEmpty(getActionParams(m_params, openId))){
                    error.Append("参数["+ openId + "]不能为空");
                    return error.ToString();
                }
            }
            
            if (string.IsNullOrEmpty(totalFee) || string.IsNullOrEmpty(orderId))
            {
                error.Append("订单号和费用不能为空");
            }
            else if (orderId.Contains("|") || !int.TryParse(totalFee, out _totalFee) || totalFee.ToString().Contains("."))
            {
                error.Append("订单号不能包含“|”，费用值必须为>=1的整数且不包含小数点");
            }
            else
            {
                WxPayData data = new WxPayData();
                data.SetValue("out_trade_no", orderId);
                data = WxPayApi.OrderQuery(data);//查询订单

                if (data.GetValue("return_code").ToString() == "FAIL")
                {
                    error.Append(data.GetValue("return_msg").ToString());
                    //return Redirect(String.Concat("http://", "www.changchunamy.com", "/Common/Error?error=" + error));
                }
                else if (data.GetValue("result_code").ToString() == "SUCCESS")
                {
                    tradeState.TradeState = data.GetValue("trade_state").ToString();
                    error.Append("商户订单号:" + orderId);
                    error.Append(",查询订单[" + orderId + "]已存在[" + tradeState.TradeState + "],请返回商城" +
                        "重新下单或联系客服处理此订单");
                }
            }
            #endregion
            return error.ToString();
        }

        /// <summary>
        /// 获得所有参数
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private SortedDictionary<string,string> getActionParams(ActionExecutingContext filterContext)
        {
            SortedDictionary<string, string> _paramsDic = new SortedDictionary<string, string>();
            //if (filterContext.HttpContext.Request.RequestType == "GET")
            //{
            //    for (int i = 0; i < filterContext.HttpContext.Request.QueryString.Count; i++)
            //    {
            //        string key = filterContext.HttpContext.Request.QueryString.Keys[i];
            //        string value = filterContext.HttpContext.Request.QueryString[key];
            //        _paramsDic.Add(key, value);
            //    }
            //}
            //else if (filterContext.HttpContext.Request.RequestType == "POST")
            //{
            if (filterContext.HttpContext.Request.Form.Count > 0)
            {
                for (int i = 0; i < filterContext.HttpContext.Request.Form.Count; i++)
                {
                    string key = filterContext.HttpContext.Request.Form.Keys[i];
                    string value = filterContext.HttpContext.Request.Form[key];
                    _paramsDic.Add(key, value);
                }
                Log.Debug("FilterVaildateApiParamas Form:", string.Concat(_paramsDic));
            }
            else if (filterContext.HttpContext.Request.ContentLength > 0)
            {
                NameValueCollection nc = new NameValueCollection();
                byte[] bytes = new byte[filterContext.HttpContext.Request.ContentLength];
                System.IO.Stream stream = filterContext.HttpContext.Request.InputStream;
                stream.Read(bytes, 0, filterContext.HttpContext.Request.ContentLength);
                System.Text.UTF8Encoding converter = new System.Text.UTF8Encoding();
                string postData = converter.GetString(bytes);
                nc = HttpUtility.ParseQueryString(postData);

                for (int i = 0; i < nc.AllKeys.Length; i++)
                {
                    string key = nc.AllKeys[i];
                    string value = nc[key];
                    _paramsDic.Add(key, value);
                }
                Log.Debug("FilterVaildateApiParamas InputStream:", string.Concat(_paramsDic));
            }
            else
            {
                for (int i = 0; i < filterContext.HttpContext.Request.QueryString.Count; i++)
                {
                    string key = filterContext.HttpContext.Request.QueryString.Keys[i];
                    string value = filterContext.HttpContext.Request.QueryString[key];
                    _paramsDic.Add(key, value);
                }
                Log.Debug("FilterVaildateApiParamas QueryString:", string.Concat(_paramsDic));
            }

            //}

            return _paramsDic;
        }

        private string getActionParams(SortedDictionary<string, string> paramsDic, string key)
        {
            if (paramsDic.ContainsKey(key))
            {
                var value = paramsDic[key];
                if (value == null)
                {
                    return "";
                }
                return value.ToString();
            }
            else
            {
                return "";
            }

            //if (filterContext.ActionParameters.ContainsKey(key))
            //{
            //    var value= filterContext.ActionParameters[key];
            //    if(value==null)
            //    {
            //        return "";
            //    }
            //    return value.ToString();
            //}
            //else
            //{
            //    return "";
            //}
        }

    }
}
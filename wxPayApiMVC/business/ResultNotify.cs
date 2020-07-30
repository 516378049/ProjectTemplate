using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI.lib;
using wxPayApiMVC.lib;

namespace WxPayAPI
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify:Notify
    {
        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());

                

                HttpContext.Current.Response.Write(res.ToXml());
                Log.Info(this.GetType().ToString(), "HttpContext.Current.Response.Write(res.ToXml());");
                HttpContext.Current.Response.End();
                Log.Info(this.GetType().ToString(), "HttpContext.Current.Response.End();");

                //将支付信息同步发送给商城
                DemoConfig dConfig = new DemoConfig();
                string resFromMall = "";
                if (!string.IsNullOrEmpty(dConfig.GetMallNotifyUrl()))
                {
                    Log.Info(this.GetType().ToString(), "开始转发微信支付结果通知给商城..." + "url：" + dConfig.GetMallNotifyUrl() + "，reXML：" + notifyData.wxResultBuilder.ToString());
                    string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                    string accesstoken=CacheHelper.GetCache("accesstoken" + out_trade_no).ToString();
                    string UserId = CacheHelper.GetCache("Id" + out_trade_no).ToString(); 
                    resFromMall = HttpService.Post(notifyData.wxResultBuilder.ToString(), dConfig.GetMallNotifyUrl(), false, 6, accesstoken, UserId);
                    Log.Info(this.GetType().ToString(), "微信支付结果通知转发给商城完毕...");
                }
                else
                {
                    Log.Info(this.GetType().ToString(), "未设置商城支付回调地址,无有发送商城通知");
                }
                Log.Info(this.GetType().ToString(), "Receive data from wxMall : " + resFromMall);

            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
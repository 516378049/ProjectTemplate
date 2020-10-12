using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using WebAPI.Core.WebAPI;
using WeChat.Common;
using WeChat.DataAccess.WeiXin.Public;

namespace OrderMeal.Controllers
{
    public class wxApiController : BaseApiController
    {
        #region default fundation
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        #endregion

        #region Sign签名算法,JSAPI
        [HttpGet]
        public ApiResult JSInit(string currentURL)
        {
            try
            {

                Log.ILog4_Debug.Debug("JSInit currentURL：" + currentURL);
                Log.ILog4_Debug.Debug("JSInit currentURL 转换后：" + currentURL.Split("#".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0]);

                string jsToken = string.Empty;
                string AccountID = string.Empty;
                AccountID = ConfigHelper.AppID;
                jsToken = RedisHelper.getRedisServer.StringGet(ConfigHelper.JsapiTicket);

                var nonceStr = GetRandomUInt().ToString();
                var timestamp = DateTime.Now.Ticks.ToString();
                var source = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsToken, nonceStr, timestamp, currentURL.Split("#".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0]);
                JsInitResponse result = new JsInitResponse()
                {
                    appId = AccountID,
                    nonceStr = nonceStr,
                    signature = WXBizMsgCrypt.GenarateSignature(source),
                    timestamp = timestamp
                };
                Log.ILog4_Debug.Debug("获取JS签名参数：" + JsonHelper.ToJson(result));
                return CreateApiResult(result);
            }
            catch (Exception e)
            {
                Log.ILog4_Error.Error("JSInit出错啦", e);
                if(currentURL.IndexOf("localhost:8080")>0)
                {
                    return CreateApiResult("fail");
                }
                throw;
            }

        }
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <returns></returns>
        public uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }
        private byte[] GenerateRandomBytes(int bytesNumber)
        {
            var csp = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[bytesNumber];
            csp.GetBytes(buffer);
            return buffer;
        }
        #endregion

        /// <summary>
        /// 微信支付结果回调
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult wxPayNotify()
        {
            try
            {
                Log.ILog4_Debug.Debug("收到微信支付回调通知......");
                #region 接收从微信后台POST过来的数据格式(支付成功)
                //<xml><appid><![CDATA[wxba3211abca2a188c]]></appid>
                //<attach><![CDATA[null]]></attach>
                //<bank_type><![CDATA[SPDB_CREDIT]]></bank_type>
                //<cash_fee><![CDATA[100]]></cash_fee>
                //<fee_type><![CDATA[CNY]]></fee_type>
                //<is_subscribe><![CDATA[Y]]></is_subscribe>
                //<mch_id><![CDATA[1544571151]]></mch_id>
                //<nonce_str><![CDATA[3659339946]]></nonce_str>
                //<openid><![CDATA[oDLo50s4l5A8E0F3OYVqSub13Wdw]]></openid>
                //<out_trade_no><![CDATA[202065231545]]></out_trade_no>
                //<result_code><![CDATA[SUCCESS]]></result_code>
                //<return_code><![CDATA[SUCCESS]]></return_code>
                //<sign><![CDATA[A0C8948B7F8ABAE297F4189F6147392408606BA06BF071C82E5F06525FC14910]]></sign>
                //<time_end><![CDATA[20200717231607]]></time_end>
                //<total_fee>100</total_fee>
                //<trade_type><![CDATA[JSAPI]]></trade_type>
                //<transaction_id><![CDATA[4200000611202007177386051392]]></transaction_id>
                //</xml>
                #endregion
                string postData = GetRequestStreamData();
                Log.ILog4_Debug.Debug("收到微信支付回调通知：\n" + postData);
                SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
                m_values = XmlHelper.FromXml(postData);
                //2015-06-29 错误是没有签名
                if (m_values["return_code"].ToString() == "SUCCESS")
                {
                    //查询微信订单
                    string orderId = m_values["out_trade_no"].ToString();
                    string url =ConfigHelper.wxOrderQuery +"?orderId=" + orderId;
                    string re=WxPayAPI.HttpService.Get(url);
                    Log.ILog4_Debug.Debug("订单在微信商户查询到结果："+ re);
                    string reStatus=JsonHelper.ConvertJsonResult(re, "data");
                    Log.ILog4_Debug.Debug("订单在微信商户查询到结果：" + reStatus);
                    if (reStatus == "SUCCESS")
                    {
                        //改变订单状态
                        Model.EF.OrderInfo orderinfo = Studio.OrderInfo.Get(X=>X.OrderNum == orderId && X.DelFlag==0).FirstOrDefault();
                        if (orderinfo != null)
                        {
                            orderinfo.transactionId = m_values["transaction_id"].ToString();
                            orderinfo.Status = 2;
                            Studio.OrderInfo.Update(orderinfo);
                            Log.ILog4_Debug.Debug("支付成功，订单状态已改变");
                            return CreateApiResult("success");
                        }
                        else
                        {
                            Log.ILog4_Debug.Debug("商户单号在业务系统中不存在");
                            return CreateApiResult("-1","商户单号在业务系统中不存在","Fail");
                        }
                        
                    }
                    else
                    {
                        Log.ILog4_Debug.Debug("支付失败，订单在微信商户未查询到结果");
                    }
                }
                else
                {
                    Log.ILog4_Debug.Debug("支付失败，订单状态未改变：微信回调返回状态不为success");
                }
                
            }
            catch (Exception ex)
            {
                Log.ILog4_Error.Error("微信支付回调接口错误", ex);
                return CreateApiResult("Fail");
            }
            return CreateApiResult("Fail");
        }
    }
}
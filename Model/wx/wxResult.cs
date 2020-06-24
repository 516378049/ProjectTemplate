using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxPayAPI.wxRsult
{
    public  class tradeState: ModelBase
    {
        private static string trade_state="";
        public static string TradeState
        {
            set { trade_state = value; }
            get {
                switch (trade_state)
                {
                    case "SUCCESS":
                        return "支付成功";
                    case "REFUND":
                        return "转入退款";
                    case "NOTPAY":
                        return "未支付";
                    case "CLOSED":
                        return "已关闭";
                    case "REVOKED":
                        return "已撤销（付款码支付）";
                    case "USERPAYING":
                        return "用户支付中（付款码支付）";
                    case "PAYERROR":
                        return "支付失败(其他原因，如银行返回失败)";
                    default:
                        return "未识别的订单状态";
                        
                }
            }
        }
    }

    public class UserInfo : ModelBase
    {
        #region user base info
        public string openid { set; get; }
        public string nickname { set; get; }
        /// <summary>
        /// 1男性，2女性，0未知
        /// </summary>
        public int sex { set; get; }
        public string province { set; get; }
        public string city { set; get; }
        public string country { set; get; }
        public string headimgurl { set; get; }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public string[] privilege { set; get; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        public string unionid { set; get; }
        public string accesstoken { set; get; }
        #endregion user authorize info
    }
    public class AuthorizeInfo : ModelBase
    {
        /// <summary>
        /// authorize code, use only onece and expired after 5 minutes
        /// </summary>
        public string code { set; get; }
        #region user authorize info
        public string access_token { set; get; }
        public int expires_in { set; get; }
        public string refresh_token { set; get; }
        public string openid { set; get; }
        public string scope { set; get; }
        public DateTime authorizeTime { set; get; }
        #endregion
    }
    /// <summary>
    /// 用户商城下单的金额和单号
    /// </summary>
    public class UserPayInfo : ModelBase
    {
        public UserPayInfo() { }
        public UserPayInfo(string openid,int totle_fee,string orderid) { this.openid = openid; this.totle_fee = totle_fee; this.orderid = orderid; }
        public string openid { set; get; }
        public int totle_fee { set; get; }
        public string orderid { set; get; }
    }

    #region 小程序
    /// <summary>
    /// 用户商城下单的金额和单号
    /// </summary>
    public class AuthCode2Session : ModelBase
    {
        public string openid { set; get; }//用户唯一标识
        public string session_key { set; get; }//会话密钥
        public string unionid { set; get; }//用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回，详见 UnionID 机制说明。
        public int errcode { set; get; }//错误码
        public string errmsg { set; get; }//错误信息
    }

    /// <summary>
    /// 授权凭证
    /// </summary>
    public class Token : ModelBase
    {
        public string access_token { set; get; }//获取到的凭证
        public int expires_in { set; get; }//凭证有效时间，单位：秒。目前是7200秒之内的值。
        public int errcode { set; get; }//错误码
        public string errmsg { set; get; }//错误信息
    }
    #endregion
}
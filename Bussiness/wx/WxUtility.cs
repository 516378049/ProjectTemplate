using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPayAPI;
using WxPayAPI.wxRsult;
using wxPayApiMVC.lib;

namespace Bussiness.wx
{
    public class WxUtility
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();
        //单例模式获取redis实例
        private static WxUtility obj = null;
        public static WxUtility GetObj
        {
            get
            {
                if (obj == null)
                {
                    lock (Locker)
                    {
                        if (obj == null)
                        {
                            obj = new WxUtility();
                        }
                    }
                }
                return obj;
            }
        }
        /**
       * 
       * 通过code换取网页授权access_token和openid的返回数据，正确时返回的JSON数据包如下：
       * {
       *  "access_token":"ACCESS_TOKEN",
       *  "expires_in":7200,
       *  "refresh_token":"REFRESH_TOKEN",
       *  "openid":"OPENID",
       *  "scope":"SCOPE",
       *  "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
       * }
       * 其中access_token可用于获取共享收货地址
       * openid是微信支付jsapi支付接口统一下单时必须的参数
       * 更详细的说明请参考网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
       * @失败时抛异常WxPayException
       */
        public UserInfo GetOpenidAndAccessTokenFromCode(string code)
        {
            try
            {
                #region 构造获取openid及access_token的url
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.GetConfig().GetAppID());
                data.SetValue("secret", WxPayConfig.GetConfig().GetAppSecret());
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();
                //请求url以获取数据
                string result = HttpService.Get(url);
                Log.Debug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + result);
                //保存access_token，用于收货地址获取
                JsonData jd = JsonMapper.ToObject(result);
                #endregion

                string access_token = (string)jd["access_token"];
                string openid = (string)jd["openid"];

                #region 记录用户授权授，模式必须为snsapi_userinfo，保存用户基本信息
                UserInfo info = wxPageApi.GetUserInfo(access_token, openid, WxPayConfig.GetConfig().GetLang());
                info.accesstoken = access_token;
                Log.UserAuthorizeInfo("GetUserInfo", "UserInfo : " + Newtonsoft.Json.JsonConvert.SerializeObject(info));//记录用户授权后的用户信息
                Log.UserAuthorizeInfo("GetUserInfo", "oauthrizeInfo : " + result);//记录用户授权后的用户信息
                Log.UserAuthorizeInfo("GetUserInfo", "end~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ : " + result);//记录用户授权后的用户信息
                #endregion

                return info;
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }
        }
    }
}

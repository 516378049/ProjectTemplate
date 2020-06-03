using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxPayAPI;
using WxPayAPI.lib;
using WxPayAPI.wxRsult;

namespace wxPayApiMVC.lib
{
    public class wxPageApi
    {
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string access_token, string appid, string lang)
        {
            UserInfo userinfo = new UserInfo();
            //AuthorizeInfo authorizeInfo = (AuthorizeInfo)HttpContext.Current.Session["authorizeInfo"];
            //string url = string.Format(wxApiUrl.userinfo(), authorizeInfo.openid, authorizeInfo.access_token, WxPayConfig.GetConfig().GetLang());
            string url = string.Format(wxApiUrl.userinfo(), access_token, appid, WxPayConfig.GetConfig().GetLang());
            Log.Debug("WxWebApi", "GetUserInfo request : " + url);
            string response = HttpService.Get(url);
            Log.Debug("WxWebApi", "GetUserInfo response : " + response);
            userinfo = JsonMapper.ToObject<UserInfo>(response);
            //HttpContext.Current.Session["userinfo"] = Newtonsoft.Json.JsonConvert.SerializeObject(userinfo);
            return userinfo;
        }

        /// <summary>
        /// 判断accessToken是否有效
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static bool authAccessToken(string appid,string access_token)
        {
            string url = string.Format(wxApiUrl.check_access_token(), access_token, appid);

            Log.Debug("WxWebApi", "authAccessToken request : " + url);
            string response = HttpService.Get(url);
            Log.Debug("WxWebApi", "authAccessToken response : " + response);

            JsonData jd = JsonMapper.ToObject(response);
            if (jd["errcode"].ToString() == "0")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 刷新AccessToken
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public static AuthorizeInfo refreshToken(string appid, string refresh_token)
        {
            string url = string.Format(wxApiUrl.refresh_token(), appid, "refresh_token", refresh_token);
            Log.Debug("WxWebApi", "authAccessToken request : " + url);
            string response = HttpService.Get(url);
            Log.Debug("WxWebApi", "authAccessToken response : " + response);
            AuthorizeInfo info = new AuthorizeInfo();
            info = JsonMapper.ToObject<AuthorizeInfo>(response);
            return info;
        }

        #region 小程序
        /// <summary>
        /// 小程序
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public static AuthCode2Session authCode2Session(string appid, string secret,string js_code,string grant_type)
        {
            string url = string.Format(wxApiUrl.auth_code2Session(), appid, secret, js_code, grant_type);
            Log.Debug("WxWebApi", "authCode2Session request : " + url);
            string response = HttpService.Get(url);
            Log.Debug("WxWebApi", "authCode2Session response : " + response);
            AuthCode2Session info = new AuthCode2Session();
            info = JsonMapper.ToObject<AuthCode2Session>(response);
            return info;
        }

        /// <summary>
        /// 小程序获取accesToken  
        /// </summary>
        /// <param name="grant_type">填写client_credential</param>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static Token token(string grant_type, string appid, string secret)
        {
            string url = string.Format(wxApiUrl.token(), grant_type, appid, secret);
            Log.Debug("WxWebApi", "token request : " + url);
            string response = HttpService.Get(url);
            Log.Debug("WxWebApi", "token response : " + response);
            Token info = new Token();
            info = JsonMapper.ToObject<Token>(response);
            return info;
        }
        #endregion
    }
}